using plugin;
using plugin.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class Plugin
    {

        // # Data submission functions

        private async Task SendData()
        {
            mbApiInterface.MB_SetBackgroundTaskMessage("Submitting all data to MusicBrainz...");
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null)
            {
                return;
            }
            else
            {
                try
                {
                    Dictionary<string, Dictionary<string, string>> allTagData = new Dictionary<string, Dictionary<string, string>>();
                    List<MusicBeeTrack> tracks = await Task.Run(() => files.Select(file => new MusicBeeTrack(file)).ToList());

                    if (tracks.Count == 0)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage("No valid tracks found for submission.");
                        return;
                    };

                    List<string> tagEntityTypes = new List<string>() { "recording", "release", "release-group" };
                    foreach (string entityType in tagEntityTypes)
                    {
                        Dictionary<string, string> tracksAndTags = new Dictionary<string, string>();
                        await Task.Run(() => TrackProcessor.ProcessForTagUpload(tracks, entityType, tracksAndTags));
                        allTagData[entityType] = tracksAndTags;
                    }


                    Dictionary<string, Dictionary<string, float>> tracksAndRatings = new Dictionary<string, Dictionary<string, float>>();
                    tracksAndRatings.Add("recording", new Dictionary<string, float>());
                    tracksAndRatings.Add("release-group", new Dictionary<string, float>());
                    await Task.Run(() => { 
                        TrackProcessor.ProcessForRatingUpload(tracks, "recording", tracksAndRatings["recording"]);
                        TrackProcessor.ProcessForRatingUpload(tracks, "release-group", tracksAndRatings["release-group"]);
                    });

                    if (allTagData.Count > 0)
                    {
                        await mbz.SetTags(allTagData);
                        await Task.Delay(500); // wait half a second to avoid rate limiting
                    }
                    if (tracksAndRatings.Count > 0)
                    {
                        await mbz.SetUserRatings(tracksAndRatings);
                    }

                    if (allTagData.Count == 0 && tracksAndRatings.Count == 0)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage("No valid data found for submission.");
                        return;
                    }
                    mbApiInterface.MB_SetBackgroundTaskMessage("Successfully submitted all data to MusicBrainz.");

                }
                catch (MusicBeeTrack.UnsupportedFormatException e)
                {
                    mbApiInterface.MB_SetBackgroundTaskMessage("Rating submission failed due to unsupported format.");
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (InvalidOperationException e)
                {
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mbApiInterface.MB_SetBackgroundTaskMessage("Rating submission failed due to an error during processing.");
                }
            }
        }

        private async Task GetAndSaveData()
        {
            // The MusicBrainz API allows 50 requests per second. The best thing to do is to split by an arbitrary amount and wait a second between each batch.
            // Process the list of MBIDs. if the amount is over a certain threshold (which'll be 50), split it up.
            mbApiInterface.MB_SetBackgroundTaskMessage($"Requesting ratings and tags from MusicBrainz...");
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null)
            {
                return;
            }

            try
            {
                List<MusicBeeTrack> tracks = await Task.Run(() => files.Select(file => new MusicBeeTrack(file)).ToList());

                // Dictionary of MBIDs and MusicBee tracks, so the ratings can be added to the correct tracks after retrieval.
                Dictionary<string, List<MusicBeeTrack>> mbidTrackPairs = new Dictionary<string, List<MusicBeeTrack>>();
                // list of MBIDs to query MusicBrainz with - prevents duplicate queries and any potential rate limiting/DDoS.
                Dictionary<int, List<string>> onlineMbidBatches = new Dictionary<int, List<string>>();

                // 1. make the batches.
                await Task.Run(() =>
                {
                    onlineMbidBatches = TrackProcessor.ProcessForDataRetrieval(tracks, mbidTrackPairs);
                });
                if (onlineMbidBatches == null)
                {
                    mbApiInterface.MB_SetBackgroundTaskMessage("No MusicBrainz data was found in the requested tracks.");
                    return;
                }

                // 2. retrieve the data from MusicBrainz.
                RetrievedMBData data = new RetrievedMBData();
                foreach (var batch in onlineMbidBatches)
                {
                    await mbz.GetAllData(batch.Value, data);
                    await Task.Delay(1100); // wait just over a second to avoid rate limiting.
                }

                // 3. save the tag data.
                foreach (MusicBeeTrack track in tracks)
                {
                    // prepare the tools needed to save the tags to the correct fields
                    Dictionary<MetaDataType, HashSet<string>> writtenTags = new Dictionary<MetaDataType, HashSet<string>>();
                    Dictionary<string, string> mbids = new Dictionary<string, string>()
                    {
                        { "recording", track.MusicBrainzRecordingId },
                        { "release", track.MusicBrainzReleaseId },
                        { "release-group", track.MusicBrainzReleaseGroupId }
                    };

                    // for each entity/MBID pair above, process tags + genres.
                    foreach (var entityMbidPair in mbids)
                    {
                        if (Settings.Default.separateGenres)
                            ProcessTagForSaving(data, entityMbidPair, writtenTags, useGenres: true);
                        ProcessTagForSaving(data, entityMbidPair, writtenTags, useGenres: false);

                        if (entityMbidPair.Key != "release")
                            ProcessRatingForSaving(data, entityMbidPair, writtenTags); // ratings can go here. just check for release.
                    }

                    // save the tags to the MusicBee track.
                    foreach (var writtenTagsPair in writtenTags)
                    {
                        mbApiInterface.Library_SetFileTag(track.FilePath, writtenTagsPair.Key, string.Join(";", writtenTagsPair.Value));
                    }
                    mbApiInterface.Library_CommitTagsToFile(track.FilePath);

                }

                mbApiInterface.MB_SetBackgroundTaskMessage("Done.");

            }
            catch (MusicBeeTrack.UnsupportedFormatException e)
            {
                mbApiInterface.MB_SetBackgroundTaskMessage("Rating retrieval failed due to unsupported format.");
                MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessRatingForSaving
            (RetrievedMBData data,
            KeyValuePair<string, string> entityMbidPair,
            Dictionary<MetaDataType, HashSet<string>> writtenTags) 
        {
            if (!string.IsNullOrWhiteSpace(entityMbidPair.Value))
            {
                
                if (data.Ratings.ContainsKey(entityMbidPair.Key) && data.Ratings[entityMbidPair.Key].ContainsKey(entityMbidPair.Value))
                {
                    float rating = data.Ratings[entityMbidPair.Key][entityMbidPair.Value];
                    MetaDataType currentMetadataType = (entityMbidPair.Key == "release-group") ? MetaDataType.RatingAlbum : MetaDataType.Rating;
                    if (!writtenTags.ContainsKey(currentMetadataType))
                    {
                        writtenTags.Add(currentMetadataType, new HashSet<string>());
                    }
                    writtenTags[currentMetadataType].Add(rating.ToString());
                }
            }
            
        }

        private void ProcessTagForSaving(RetrievedMBData data,
            KeyValuePair<string, string> entityMbidPair,
            Dictionary<MetaDataType, HashSet<string>> writtenTags,
            bool useGenres)
        {
            if (!string.IsNullOrWhiteSpace(entityMbidPair.Value))
            {
                Dictionary<string, Dictionary<string, List<string>>> chosenDataDict = (useGenres ? data.Genres : data.Tags);
                string tagFieldKey = entityMbidPair.Key + (useGenres ? "-genre" : "-tag");
                if (chosenDataDict.ContainsKey(entityMbidPair.Key) && chosenDataDict[entityMbidPair.Key].ContainsKey(entityMbidPair.Value))
                {
                    foreach (var tag in chosenDataDict[entityMbidPair.Key][entityMbidPair.Value])
                    {
                        MetaDataType? chosenMetadataType = AssignTagField(tagFieldKey, tag);
                        if (chosenMetadataType != null)
                        {
                            if (!writtenTags.ContainsKey(chosenMetadataType.Value))
                            {
                                writtenTags.Add(chosenMetadataType.Value, new HashSet<string>());
                            }
                            writtenTags[chosenMetadataType.Value].Add(tag);
                        }
                    }
                }
            }
        }

        private MetaDataType? AssignTagField(string tagKey, string tag)
        {
            Debug.WriteLine($"AssignTagField executed - TagMoveEnabled: {Settings.Default.tagMoveEnabled}, TagsToMove:\n{Settings.Default.tagsToMove}");
            string currentTagKey = tagKey;
            
            if (Settings.Default.tagMoveEnabled && !string.IsNullOrWhiteSpace(Settings.Default.tagsToMove))
            {
                // Dictionary<string, string> tagMovePairs = new Dictionary<string, string>();
                Debug.WriteLine("We're in the tagMoveEnabled branch.");
                foreach (string line in Settings.Default.tagsToMove.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] parts = line.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        string sourceTag = parts[0].Trim();
                        string targetField = parts[1].Trim();
                        Debug.WriteLine($"Source tag: {sourceTag}, target field: {targetField}");
                        if (tag.ToLowerInvariant() == sourceTag)
                        {
                            switch (targetField)
                            {
                                case "current_genre":
                                    currentTagKey = tagKey.Split('-')[0] + "-genre";
                                    break;
                                case "current_tag":
                                    currentTagKey = tagKey.Split('-')[0] + "-tag";
                                    break;
                                case "delete":
                                    return null;
                                default:
                                    // execution will stop here if current-genre or current-tag aren't specified in the tag field assignment settings.
                                    Debug.WriteLine(targetField);
                                    if (tagBindings.ContainsKey(targetField))
                                        return tagBindings[targetField];
                                    else
                                        throw new ArgumentException($"Invalid target field: {targetField}");
                            }
                        }
                    }
                }
            }

            if (!Settings.Default.separateFieldsByEntityType)
            {
                if (currentTagKey.Contains("-genre") && Settings.Default.separateGenres)
                    return tagBindings[Settings.Default.recordingGenreField];
                else
                    return tagBindings[Settings.Default.recordingTagField];
            }

            switch (currentTagKey)
            {
                case "recording-tag":
                    return tagBindings[Settings.Default.recordingTagField];
                case "release-tag":
                    return tagBindings[Settings.Default.releaseTagField];
                case "release-group-tag":
                    return tagBindings[Settings.Default.releaseGroupTagField];
                case "recording-genre":
                    return Settings.Default.separateGenres
                        ? tagBindings[Settings.Default.recordingGenreField]
                        : tagBindings[Settings.Default.recordingTagField];
                case "release-genre":
                    return Settings.Default.separateGenres
                        ? tagBindings[Settings.Default.releaseGenreField]
                        : tagBindings[Settings.Default.releaseTagField];
                case "release-group-genre":
                    return Settings.Default.separateGenres
                        ? tagBindings[Settings.Default.releaseGroupGenreField]
                        : tagBindings[Settings.Default.releaseGroupTagField];
                default:
                    throw new ArgumentException($"Invalid key: {currentTagKey}");
            }
        }
    }
}

using plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class Plugin
    {

        // # Data submission functions

        /// <summary>
        /// Processes the track or album's rating data to be sent to MusicBrainz.
        /// </summary>
        private async Task SendRatingData(string entity_type, bool reset = false)
        {
            mbApiInterface.MB_SetBackgroundTaskMessage($"Submitting {entity_type.Replace('-', ' ')} ratings to MusicBrainz...");
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null)
            {
                return;
            }
            else
            {
                try
                {
                    Dictionary<string, float> tracksAndRatings = new Dictionary<string, float>();
                    List<MusicBeeTrack> tracks = await Task.Run(() => files.Select(file => new MusicBeeTrack(file)).ToList());
                    // required to check album ratings for consistency.
                    await Task.Run(() => TrackProcessor.ProcessForRatingUpload(tracks, entity_type, tracksAndRatings, reset));
                    if (tracksAndRatings.Count == 0)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage("Ratings not submitted due to no retrievable MusicBrainz IDs.");
                    }
                    else
                    {
                        await mbz.SetUserRatings(tracksAndRatings, entity_type);
                        mbApiInterface.MB_SetBackgroundTaskMessage($"Successfully submitted {entity_type.Replace('-', ' ')} ratings to MusicBrainz.");
                    }
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

        /// <summary>
        /// Processes the track or album's tag data to be sent to MusicBrainz.
        /// </summary>
        private async Task SendTagData(string entity_type)
        {
            mbApiInterface.MB_SetBackgroundTaskMessage($"Sending {entity_type.Replace('-', ' ')} tags to MusicBrainz.");
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null)
            {
                return;
            }
            else
            {
                try
                {
                    Dictionary<string, string> tracksAndTags = new Dictionary<string, string>();
                    List<MusicBeeTrack> tracks = await Task.Run(() => files.Select(file => new MusicBeeTrack(file)).ToList());
                    await Task.Run(() => TrackProcessor.ProcessForTagUpload(tracks, entity_type, tracksAndTags));
                    if (tracksAndTags.Count == 0)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage("Tags not submitted due to empty data.");
                    }
                    else
                    {
                        await mbz.SetTags(tracksAndTags, entity_type);
                        mbApiInterface.MB_SetBackgroundTaskMessage($"Successfully submitted {entity_type.Replace('-', ' ')} tags to MusicBrainz.");
                    }
                }
                catch (MusicBeeTrack.UnsupportedFormatException e)
                {
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mbApiInterface.MB_SetBackgroundTaskMessage("Tag submission failed due to unsupported format.");
                }
                catch (InvalidOperationException e)
                {
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mbApiInterface.MB_SetBackgroundTaskMessage("Tag submission failed due to an error during processing.");
                }
            }
        }

        /// <summary>
        /// Processes track and/or album IDs to retrieve the currently logged in user's ratings.
        /// </summary>
        private async Task GetRatingData(string entity_type)
        {
            // The MusicBrainz API allows 300 requests per second. The best thing to do is to split by an arbitrary amount and wait a second between each batch.
            // Process the list of MBIDs. if the amount is over a certain threshold (which'll be 50), split it up.
            mbApiInterface.MB_SetBackgroundTaskMessage($"Requesting {entity_type.Replace('-', ' ')} ratings from MusicBrainz...");
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null)
            {
                return;
            }
            else
            {
                try
                {
                    List<MusicBeeTrack> tracks = await Task.Run(() => files.Select(file => new MusicBeeTrack(file)).ToList());

                    // Dictionary of MBIDs and MusicBee tracks, so the ratings can be added to the correct tracks after retrieval.
                    Dictionary<string, List<MusicBeeTrack>> mbidTrackPairs = new Dictionary<string, List<MusicBeeTrack>>();
                    // list of MBIDs to query MusicBrainz with - prevents duplicate queries and any potential rate limiting/DDoS.
                    Dictionary<int, List<string>> onlineMbidBatches = new Dictionary<int, List<string>>();

                    await Task.Run(() =>
                    {
                        onlineMbidBatches = TrackProcessor.ProcessForDataRetrieval(MusicBrainzDataType.UserRatings, mbApiInterface, tracks, entity_type, mbidTrackPairs);
                    });

                    if (onlineMbidBatches == null) return;

                    Dictionary<string, int?> albumsAndRatings = new Dictionary<string, int?>();
                    foreach (var batch in onlineMbidBatches)
                    {
                        var batchRatings = await mbz.GetUserRatings(batch.Value);
                        foreach (var pair in batchRatings)
                        {
                            albumsAndRatings.Add(pair.Key, pair.Value);
                        }
                        await Task.Delay(1100); // wait just over a second to avoid rate limiting.
                    }

                    if (albumsAndRatings == null) return;
#if DEBUG
                    foreach (var pair in albumsAndRatings)
                    {
                        Debug.WriteLine($"[Plugin.GetRatingData] MBID: {pair.Key}, Rating: {pair.Value}");
                    }
#endif
                    int editedRatingCounter = 0;
                    await Task.Run(() =>
                    {
                        foreach (var pair in albumsAndRatings)
                        {
                            // don't want to do anything if the user hasn't rated it, so check if the key exists in mbidTrackPairs.
                            if (mbidTrackPairs.ContainsKey(pair.Key))
                            {
                                foreach (MusicBeeTrack track in mbidTrackPairs[pair.Key])
                                {
                                    MetaDataType chosenMetadata = (entity_type == "release-group") ? MetaDataType.RatingAlbum : MetaDataType.Rating;
                                    mbApiInterface.Library_SetFileTag(track.FilePath, chosenMetadata, pair.Value.ToString());
                                    mbApiInterface.Library_CommitTagsToFile(track.FilePath);
                                    editedRatingCounter++;
                                }
                            }

                        }
                    });

                    if (editedRatingCounter != 0)
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage($"Successfully retrieved {entity_type.Replace('-', ' ')} ratings from MusicBrainz.");
                    }
                    else
                    {
                        mbApiInterface.MB_SetBackgroundTaskMessage($"No ratings were saved on MusicBrainz for the selected {entity_type.Replace('-', ' ')}s.");
                    }

                }
                catch (MusicBeeTrack.UnsupportedFormatException e)
                {
                    mbApiInterface.MB_SetBackgroundTaskMessage("Rating retrieval failed due to unsupported format.");
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private async Task GetTagData(string entityType)
        {
            mbApiInterface.MB_SetBackgroundTaskMessage($"Requesting {entityType.Replace('-', ' ')} tags from MusicBrainz...");
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null)
            {
                return;
            }
            else
            {
                try
                {
                    List<MusicBeeTrack> tracks = await Task.Run(() => files.Select(file => new MusicBeeTrack(file)).ToList());

                    // Dictionary of MBIDs and MusicBee tracks, so the ratings can be added to the correct tracks after retrieval.
                    // We may be duplicating these functions between GetTagData and GetRatingData, so it might be worth refactoring later.
                    Dictionary<string, List<MusicBeeTrack>> mbidTrackPairs = new Dictionary<string, List<MusicBeeTrack>>();
                    // list of MBIDs to query MusicBrainz with - prevents duplicate queries and any potential rate limiting/DDoS.
                    Dictionary<int, List<string>> onlineMbidBatches = new Dictionary<int, List<string>>();
                    Dictionary<string, Dictionary<string, List<string>>> mbidsAndTags = new Dictionary<string, Dictionary<string, List<string>>>();

                    await Task.Run(() =>
                    {
                        onlineMbidBatches = TrackProcessor.ProcessForDataRetrieval(MusicBrainzDataType.UserTags, mbApiInterface, tracks, entityType, mbidTrackPairs);
                    });

                    if (onlineMbidBatches == null) return;

                    foreach (var batch in onlineMbidBatches)
                    {
                        var batchTags = await mbz.GetTags(batch.Value);
                        foreach (var pair in batchTags)
                        {
                            mbidsAndTags.Add(pair.Key, pair.Value);
                        }
                        await Task.Delay(1100); // wait just over a second to avoid rate limiting.
                    }
                    await Task.Run(() =>
                    {
                        foreach (var pair in mbidsAndTags)
                        {
                            if (mbidTrackPairs.ContainsKey(pair.Key))
                            {
                                foreach (MusicBeeTrack track in mbidTrackPairs[pair.Key])
                                {
                                    foreach (var tagAndValues in pair.Value) // gonna need to find a new name for this variable
                                    {
                                        MetaDataType currentTag = tagBindings[tagAndValues.Key];
                                        mbApiInterface.Library_SetFileTag(track.FilePath, currentTag, string.Join("; ", tagAndValues.Value));
                                    }
                                    mbApiInterface.Library_CommitTagsToFile(track.FilePath);
                                    // Debug.WriteLine($"[Plugin.GetTagData] Successfully committed edited tags to {track.Artist} - {track.Title}");

                                }
                            }
                        }

                    });
                    mbApiInterface.MB_SetBackgroundTaskMessage($"Successfully retrieved {entityType.Replace('-', ' ')} tags from MusicBrainz.");
                }
                catch (MusicBeeTrack.UnsupportedFormatException e)
                {
                    mbApiInterface.MB_SetBackgroundTaskMessage("Tag retrieval failed due to unsupported format.");
                    MessageBox.Show($"Error: {e.Message}", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

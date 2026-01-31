using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace plugin
{
    public enum MusicBrainzDataType
    {
        UserRatings,
        UserTags
    }

    internal static class TrackProcessor
    {
        public static Dictionary<int, List<string>> ProcessForDataRetrieval(MusicBrainzDataType type, MusicBeeApiInterface mbApiInterface, List<MusicBeeTrack> tracks, string entityType, Dictionary<string, List<MusicBeeTrack>> mbidTrackPairs)
        {
            List<string> onlineMbids = new List<string>();
            foreach (MusicBeeTrack track in tracks)
            {
                string currentMbid;
                string currentOnlineMbid;
                // todo merge this switch with the one in ProcessForTagDataRetrieval since they're almost identical
                // use if to switch between tags and ratings instead of an entirely new method.

                if (type == MusicBrainzDataType.UserRatings)
                {
                    switch (entityType)
                    {
                        case "release-group":
                            if (string.IsNullOrEmpty(track.MusicBrainzReleaseGroupId)) continue;
                            // release group / album ratings
                            currentMbid = track.MusicBrainzReleaseGroupId;
                            // I could make something fancy here but lmao just prefix the ID with the entity type will do. If it ain't broke don't fix it.
                            currentOnlineMbid = "release-group/" + track.MusicBrainzReleaseGroupId;
                            break;
                        default:
                            if (string.IsNullOrEmpty(track.MusicBrainzRecordingId)) continue;
                            // the recording ID is the key regardless, but for online queries, we want to use the release ID if it exists.
                            // This makes querying for recordings tied to a specific release more reliable, preventing any potential DDoS/rate limiting.
                            currentMbid = track.MusicBrainzRecordingId;
                            if (!String.IsNullOrEmpty(track.MusicBrainzReleaseId))
                            {
                                currentOnlineMbid = "release/" + track.MusicBrainzReleaseId;
                            }
                            else
                            {
                                currentOnlineMbid = "recording/" + track.MusicBrainzRecordingId;
                            }
                            break;
                    }
                }
                else
                {
                    switch (entityType)
                    {
                        case "release-group":
                            if (string.IsNullOrEmpty(track.MusicBrainzReleaseGroupId)) continue;
                            currentMbid = track.MusicBrainzReleaseGroupId;
                            // I could make something fancy here but lmao just prefix the ID with the entity type will do. If it ain't broke don't fix it.
                            currentOnlineMbid = "release-group/" + track.MusicBrainzReleaseGroupId;
                            break;
                        case "release":
                            if (string.IsNullOrEmpty(track.MusicBrainzReleaseId)) continue;
                            currentMbid = track.MusicBrainzReleaseId;
                            currentOnlineMbid = "release/" + track.MusicBrainzReleaseId;
                            break;
                        default:
                            // for recordings where a release ID is being grabbed instead, a fake entity is being used to differentiate between that and a regular recording ID.
                            if (string.IsNullOrEmpty(track.MusicBrainzRecordingId)) continue;
                            currentMbid = track.MusicBrainzRecordingId;
                            if (!String.IsNullOrEmpty(track.MusicBrainzReleaseId))
                            {
                                currentOnlineMbid = "recording-release/" + track.MusicBrainzReleaseId;
                            }
                            else
                            {
                                currentOnlineMbid = "recording/" + track.MusicBrainzRecordingId;
                            }
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(currentMbid))
                {
                    Debug.WriteLine($"[Plugin.GetRatingData] Title: {track.Title}, {entityType} MBID: {currentMbid}");
                    Debug.WriteLine("[Plugin.GetRatingData]" + string.Join("; ", onlineMbids));
                    // currentMbids should have one track object associated with them when it comes to getting recordings,
                    // but should have all tracks on a release bundled together for RG ratings.
                    // The idea is to implement this as generically as possible.
                    if (mbidTrackPairs.ContainsKey(currentMbid))
                    {
                        mbidTrackPairs[currentMbid].Add(track);
                    }
                    else
                    {
                        mbidTrackPairs.Add(currentMbid, new List<MusicBeeTrack> { track });
                    }

                    if (!onlineMbids.Contains(currentOnlineMbid))
                    {
                        onlineMbids.Add(currentOnlineMbid);
                    }
                }

            }

            if (mbidTrackPairs.Count == 0)
            {
                mbApiInterface.MB_SetBackgroundTaskMessage($"No ratings were saved on MusicBrainz for the selected {entityType.Replace('-', ' ')}s.");
                MessageBox.Show("No ratings have been received since none of the files being processed have any MusicBrainz IDs to use. You need to match them to entries on MusicBrainz using Picard.", "MusicBrainz Sync", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            else
            {
                // I heard that MusicBrainz had a limit of 50 requests per second, wiki says 300.
                // I'll use 49 to be on the safe side.
                Dictionary<int, List<string>> onlineMbidBatches = onlineMbids
                    .Select((mbid, index) => new { mbid, index })
                    .GroupBy(x => x.index / 49)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.mbid).ToList());

                return onlineMbidBatches;
            }
        }


        public static void ProcessForTagUpload(List<MusicBeeTrack> tracks, string entityType, Dictionary<string, string> tracksAndTags)
        {
            foreach (MusicBeeTrack track in tracks)
            {
                List<string> tags = track.GetAllTagsFromFile(entityType);
                string currentMbid;
                string errorMessage = $"{track.Album} has inconsistent tags.\n\nGive every track on that album the exact same album rating and try to submit again."; ;
                switch (entityType)
                {
                    case "release":
                        currentMbid = track.MusicBrainzReleaseId;
                        break;
                    case "release-group":
                        currentMbid = track.MusicBrainzReleaseGroupId;
                        break;
                    default:
                        currentMbid = track.MusicBrainzRecordingId;
                        errorMessage = "The group of tracks you attempted to submit tags for have two track MBIDs that are the same. This is likely due to a malformed data entry to MusicBrainz.\n\nCheck that the recordings should actually be the same.";
                        break;
                }
                if (!string.IsNullOrEmpty(currentMbid) && tags != null)
                {
                    Debug.WriteLine($"[TrackProcessor.ProcessForTagUpload] Title: {track.Title}, {entityType} MBID: {currentMbid}, Tags: {string.Join("; ", tags)}");
                    
                    // this release is mainly for releases - recordings should have different MBIDs each time, while releases/RGs do not.
                    if (tracksAndTags.ContainsKey(currentMbid))
                    {
                        if (tracksAndTags[currentMbid] != String.Join(";", tags))
                        {
                            throw new InvalidOperationException(errorMessage);
                        }
                    }
                    else
                    {
                        tracksAndTags.Add(currentMbid, String.Join(";", tags));
                    }
                }
            }
        }

        public static void ProcessForRatingUpload(List<MusicBeeTrack> tracks, string entityType, Dictionary<string, float> tracksAndRatings, bool reset = false)
        {
            foreach (MusicBeeTrack track in tracks)
            {
                string currentMbid;
                string errorMessage;
                float onlineRating = 0;
                switch (entityType)
                {
                    // no need to handle releases, MusicBrainz doesn't allow rating of releases at the moment.
                    case "release-group":
                        currentMbid = track.MusicBrainzReleaseGroupId;
                        if (!string.IsNullOrEmpty(track.AlbumRating) && !reset)
                        {
                            onlineRating = float.Parse(track.AlbumRating);
                        }
                        errorMessage = $"{track.Album} has inconsistent album ratings.\n\nGive every track on that album the exact same album rating and try to submit again.";
                        break;
                    default:
                        currentMbid = track.MusicBrainzRecordingId;
                        if (!string.IsNullOrEmpty(track.Rating) && !reset)
                        {
                            onlineRating = float.Parse(track.Rating) * 20;
                        }
                        // Generally, albums on MusicBrainz shouldn't have tracks with the same recording ID.
                        errorMessage = "The group of tracks you attempted to submit ratings for have two track MBIDs that are the same. This is likely due to a malformed data entry to MusicBrainz.\n\nCheck that the recordings should actually be the same.";
                        break;
                }

                if (!string.IsNullOrEmpty(currentMbid))
                {
                    // Debug.WriteLine($"[Plugin.SendRatingData] Title: {track.Title}, {entityType} MBID: {currentMbid}, Rating: {onlineRating}");
                    if (tracksAndRatings.ContainsKey(currentMbid))
                    {
                        if (tracksAndRatings[currentMbid] != onlineRating)
                        {
                            throw new InvalidOperationException(errorMessage);
                        }
                    }
                    else
                    {
                        // setting a track to 0 will just wipe the rating from MusicBrainz, so it won't be a problem.
                        tracksAndRatings.Add(currentMbid, onlineRating);
                    }
                }
            }
        }
    }
}

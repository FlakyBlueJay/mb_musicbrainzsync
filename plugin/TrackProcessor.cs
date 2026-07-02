using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace plugin
{
    internal static class TrackProcessor
    {
        public static Dictionary<int, List<string>> ProcessForDataRetrieval(List<MusicBeeTrack> tracks, Dictionary<string, List<MusicBeeTrack>> mbidTrackPairs)
        {

            HashSet<string> onlineMbids = new HashSet<string>();
            foreach (MusicBeeTrack track in tracks) {

                if (!string.IsNullOrEmpty(track.MusicBrainzReleaseGroupId))
                {
                    if (!mbidTrackPairs.ContainsKey(track.MusicBrainzReleaseGroupId))
                        mbidTrackPairs.Add(track.MusicBrainzReleaseGroupId, new List<MusicBeeTrack> { track });
                    else
                        mbidTrackPairs[track.MusicBrainzReleaseGroupId].Add(track);
                    onlineMbids.Add("release-group/" + track.MusicBrainzReleaseGroupId);
                }

                if (!string.IsNullOrEmpty(track.MusicBrainzReleaseId))
                {
                    if (!mbidTrackPairs.ContainsKey(track.MusicBrainzReleaseId))
                        mbidTrackPairs.Add(track.MusicBrainzReleaseId, new List<MusicBeeTrack> { track });
                    else
                        mbidTrackPairs[track.MusicBrainzReleaseId].Add(track);
                    onlineMbids.Add("release/" + track.MusicBrainzReleaseId);
                }

                if (!string.IsNullOrEmpty(track.MusicBrainzRecordingId))
                {
                    if (!mbidTrackPairs.ContainsKey(track.MusicBrainzRecordingId))
                        mbidTrackPairs.Add(track.MusicBrainzRecordingId, new List<MusicBeeTrack> { track });
                    else
                        mbidTrackPairs[track.MusicBrainzRecordingId].Add(track);

                    // recording user data can be retrieved using a release, unless there's no release tag
                    if (string.IsNullOrEmpty(track.MusicBrainzReleaseId))
                        onlineMbids.Add("recording/" + track.MusicBrainzRecordingId);
                }
            }

            Dictionary<int, List<string>> onlineMbidBatches = onlineMbids
                    .ToList()
                    .Select((mbid, index) => new { mbid, index })
                    .GroupBy(x => x.index / 49)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.mbid).ToList());
            return onlineMbidBatches;
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

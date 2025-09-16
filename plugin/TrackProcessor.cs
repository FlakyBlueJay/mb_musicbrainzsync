using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plugin
{
    internal static class TrackProcessor
    {
        public static void ProcessForRatingData(List<MusicBeeTrack> tracks, string entityType, List<string> onlineMbids, Dictionary<string, List<MusicBeeTrack>> mbidTrackPairs)
        {
            foreach (MusicBeeTrack track in tracks)
            {
                if (!string.IsNullOrEmpty(track.MusicBrainzRecordingId) || !string.IsNullOrEmpty(track.MusicBrainzReleaseGroupId)) // check if the track actually has a MBID.
                {
                    string currentMbid;
                    string currentOnlineMbid;
                    switch (entityType)
                    {
                        case "release-group":
                            // release group / album ratings
                            currentMbid = track.MusicBrainzReleaseGroupId;
                            // I could make something fancy here but lmao just prefix the ID with the entity type will do. If it ain't broke don't fix it.
                            currentOnlineMbid = "release-group/" + track.MusicBrainzReleaseGroupId;
                            break;
                        default:
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
                        if (onlineMbids.Contains(currentOnlineMbid) == false)
                        {
                            onlineMbids.Add(currentOnlineMbid);
                        }
                    }
                }
            }
        }

        public static void ProcessForTagData(List<MusicBeeTrack> tracks, string entityType, List<string> onlineMbids, Dictionary<string, List<MusicBeeTrack>> mbidTrackPairs)
        {
            foreach (MusicBeeTrack track in tracks)
            {
                if (!string.IsNullOrEmpty(track.MusicBrainzRecordingId)) // check if the track actually has a MBID. Since this is evaluating by track, recording MBID is a good one to start from.
                {
                    string currentMbid = "";
                    string currentOnlineMbid = "";
                    switch (entityType)
                    {
                        case "release-group":
                            currentMbid = track.MusicBrainzReleaseGroupId;
                            // I could make something fancy here but lmao just prefix the ID with the entity type will do. If it ain't broke don't fix it.
                            currentOnlineMbid = "release-group/" + track.MusicBrainzReleaseGroupId;
                            break;
                        case "release":
                            currentMbid = track.MusicBrainzReleaseId;
                            currentOnlineMbid = "release/" + track.MusicBrainzReleaseId;
                            break;
                        default:
                            // for recordings where a release ID is being grabbed instead, a fake entity is being used to differentiate between that and a regular recording ID.
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

                    if (!string.IsNullOrEmpty(currentMbid))
                    {
                        Debug.WriteLine($"[Plugin.GetTagData] Title: {track.Title}, {entityType} MBID: {currentMbid}");
                        Debug.WriteLine("[Plugin.GetTagData]" + string.Join("; ", onlineMbids));
                        if (mbidTrackPairs.ContainsKey(currentMbid))
                        {
                            mbidTrackPairs[currentMbid].Add(track);
                        }
                        else
                        {
                            mbidTrackPairs.Add(currentMbid, new List<MusicBeeTrack> { track });
                        }
                        if (onlineMbids.Contains(currentOnlineMbid) == false)
                        {
                            onlineMbids.Add(currentOnlineMbid);
                        }
                    }


                }
            }
        }

    }
}

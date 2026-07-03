using plugin.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace plugin
{
    /// <summary>
    /// A service for processing data retrieved from the MusicBrainz API, after it has been retrieved.
    /// </summary>
    public static class MusicBrainzDataProcessor
    {
        public static Dictionary<string, List<string>> ConvertMusicBrainzTags(List<MusicBrainzTag> onlineTagData, List<MusicBrainzTag> onlineGenreData, string entityType = "recording")
        {
            // combinedTagData will have "tags" and "genres" as keys.
            Dictionary<string, List<string>> combinedTagData = new Dictionary<string, List<string>>();
            combinedTagData.Add("tags", new List<string>());
            combinedTagData.Add("genres", new List<string>());

            if (Settings.Default.separateGenres && onlineGenreData.Count > 0)
            {
                ProcessTagsToList(combinedTagData, onlineGenreData, "genres");
            }

            if (onlineTagData.Count > 0)
            {
                ProcessTagsToList(combinedTagData, onlineTagData, "tags");
            }

            foreach (string key in combinedTagData.Keys)
            {
                combinedTagData[key].Sort(StringComparer.CurrentCultureIgnoreCase);
            }

            Debug.WriteLine($"Genres: {string.Join("/", combinedTagData["genres"])}");
            Debug.WriteLine($"Tags: {string.Join("/", combinedTagData["tags"])}");
            
            return combinedTagData;
        }

        private static void ProcessTagsToList(Dictionary<string, List<string>> combinedTagData, List<MusicBrainzTag> tagList, string tagKey)
        {
            MusicBrainzTag topTag = tagList[0];
            int maxTags = (tagKey == "genres") ? Settings.Default.maxGenres : Settings.Default.maxTags;
            int countThreshold = (tagKey == "genres") ? Settings.Default.genreCountThreshold : Settings.Default.tagCountThreshold;
            tagList = tagList.OrderByDescending(tag => tag.Count).ToList();
            if (maxTags > 0)
            {
                if (tagList.Count > maxTags)
                {
                    tagList.RemoveRange(maxTags, tagList.Count - maxTags);
                }
            }
            foreach (MusicBrainzTag mbzTag in tagList)
            {
                string addedTag = FindReplaceTag(mbzTag.Name, true);
                int? percentage = null;
                if (!Settings.Default.downloadOnlyUserTags)
                    percentage = 100 * mbzTag.Count / topTag.Count;

                Debug.WriteLine($"[MusicBrainzAPI.ProcessOnlineTags] Tag: {mbzTag.Name}, Relative percentage use: {percentage} (Percentage threshold for tags: {countThreshold})");
                if ((percentage != null && percentage > countThreshold) || Settings.Default.downloadOnlyUserTags)
                {
                    addedTag = ProcessTagLetterCase(addedTag);
                    Debug.WriteLine($"[MusicBrainzAPI.ProcessOnlineTags] Tag: {mbzTag.Name}, FindReplaced: {addedTag}");
                    // do not add tag to tags if separate genres
                    if (Settings.Default.separateGenres && tagKey == "tags" && combinedTagData["genres"].Contains(addedTag)) continue;
                    combinedTagData[tagKey].Add(addedTag);
                }
            }
        }

        private static string ProcessTagLetterCase(string tag)
        {
            switch (Settings.Default.letterCaseMode)
            {
                case 2: // lower case
                    // MusicBrainz sends tags in lowercase so no need to do anything.
                    break;
                case 1: // sentence case
                    tag = tag[0].ToString().ToUpper() + tag.Substring(1);
                    break;
                default: // default, title case
                    TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                    tag = ti.ToTitleCase(tag);
                    break;
            }
            return tag;
        }

        public static string FindReplaceTag(string tag, bool reverse = false)
        {
            tag = tag.ToLower();
            if (string.IsNullOrEmpty(Settings.Default.findReplace))
            {
                return tag;
            }
            else
            {
                Dictionary<string, string> findReplace = new Dictionary<string, string>();
                // convert the findReplace setting into a dictionary.
                foreach (string line in Settings.Default.findReplace.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    // split by the first semicolon to get the find and replace values
                    string[] row = line.Split(new[] { ';' }, 2);
                    if (row.Length == 2)
                    {
                        string findTag = (!reverse) ? row[0] : row[1];
                        string replaceTag = (!reverse) ? row[1] : row[0];
                        findReplace.Add(findTag, replaceTag);
                    }
                }
                if (findReplace.ContainsKey(tag))
                {
                    return findReplace[tag];
                }
                else
                {
                    return tag;
                }
            }
        }

        // This would've been better handled by an IMusicBrainzObject but it's not worth it atm.
        public static Dictionary<string, List<string>> ProcessOnlineTags(Recording rec)
        {
            if (Settings.Default.downloadOnlyUserTags)
                return ConvertMusicBrainzTags(rec.UserTags, rec.UserGenres, "recording");

            return ConvertMusicBrainzTags(rec.Tags, rec.Genres, "recording");
        }

        public static Dictionary<string, List<string>> ProcessOnlineTags(Release r)
        {
            if (Settings.Default.downloadOnlyUserTags)
                return ConvertMusicBrainzTags(r.UserTags, r.UserGenres, "release");

            return ConvertMusicBrainzTags(r.Tags, r.Genres, "release");
        }

        public static Dictionary<string, List<string>> ProcessOnlineTags(ReleaseGroup rg)
        {
            if (Settings.Default.downloadOnlyUserTags)
                return ConvertMusicBrainzTags(rg.UserTags, rg.UserGenres, "release-group");

            return ConvertMusicBrainzTags(rg.Tags, rg.Genres, "release-group");
        }

        public static void AddRatingData(RetrievedMBData data, string mbid, double? rating, string key)
        {
            if (rating.HasValue)
            {
                double mbRating = (rating.Value * 2) * 10; // convert to 0-100 scale
                if (!data.Ratings.ContainsKey(key)) data.Ratings.Add(key, new Dictionary<string, float>());
                if (!string.IsNullOrEmpty(mbid) && !data.Ratings[key].ContainsKey(mbid))
                    data.Ratings[key].Add(mbid, (int)Math.Round(mbRating));
            }
        }

        public static void AddTagData(RetrievedMBData data,
            Dictionary<string, List<string>> tagData,
            string mbid,
            string key)
        {
            if (!data.Genres.ContainsKey(key))
                data.Genres.Add(key, new Dictionary<string, List<string>>());
            if (!data.Tags.ContainsKey(key))
                data.Tags.Add(key, new Dictionary<string, List<string>>());

            if (Settings.Default.separateGenres && !data.Genres[key].ContainsKey(mbid))
                data.Genres[key].Add(mbid, tagData["genres"]);

            // should filter out genres already added.
            foreach (string tag in tagData["tags"])
            {
                if (Settings.Default.separateGenres && tagData["genres"].Contains(tag))
                {
                    // don't add tag if separateGenres enabled + already in genres, continue
                    continue;
                }
                if (!data.Tags[key].ContainsKey(mbid))
                {
                    data.Tags[key].Add(mbid, new List<string>());
                }
                data.Tags[key][mbid].Add(tag);
            }
        }

        public static void ProcessRecordingObjects(RetrievedMBData data, List<Recording> recs)
        {
            foreach (Recording rec in recs)
            {
                AddRatingData(data, rec.MBID, rec.CurrentUserRating, "recording");
                Dictionary<string, List<string>> tagData = MusicBrainzDataProcessor.ProcessOnlineTags(rec);
                AddTagData(data, tagData, rec.MBID, "recording");
            }

        }

        public static string RatingDataToXML(Dictionary<string, Dictionary<string, float>> mbidRatings)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);

            xmlWriter.WriteStartElement("metadata"); xmlWriter.WriteAttributeString("xmlns", "http://musicbrainz.org/ns/mmd-2.0#");
            foreach (KeyValuePair<string, Dictionary<string, float>> entityTypePair in mbidRatings)
            {
                string entityType = entityTypePair.Key;
                Dictionary<string, float> mbidRatingPairs = entityTypePair.Value;
                xmlWriter.WriteStartElement($"{entityType}-list");
                foreach (KeyValuePair<string, float> trackRatingPair in mbidRatingPairs)
                {
                    string mbid = trackRatingPair.Key;
                    string rating = trackRatingPair.Value.ToString();
                    xmlWriter.WriteStartElement(entityType); xmlWriter.WriteAttributeString("id", mbid);
                    xmlWriter.WriteElementString("user-rating", rating);
                    xmlWriter.WriteEndElement(); // ends individual recording XML
                }
                xmlWriter.WriteEndElement(); // ends entity list XML
            }
            xmlWriter.WriteEndElement(); // ends metadata XML
            xmlWriter.Flush();
            string xmlData = stringWriter.ToString();
            return xmlData;
        }

        public static string TagDataToXML(Dictionary<string, Dictionary<string, string>> trackMbid_TagPairing)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);

            xmlWriter.WriteStartElement("metadata"); xmlWriter.WriteAttributeString("xmlns", "http://musicbrainz.org/ns/mmd-2.0#");
            foreach (KeyValuePair<string, Dictionary<string, string>> entityTypePair in trackMbid_TagPairing)
            {
                string entityType = entityTypePair.Key;
                Dictionary<string, string> mbidTagPairs = entityTypePair.Value;
                xmlWriter.WriteStartElement($"{entityType}-list");
                foreach (KeyValuePair<string, string> trackTagPair in mbidTagPairs)
                {
                    string recordingMbid = trackTagPair.Key;
                    string tag = trackTagPair.Value;
                    xmlWriter.WriteStartElement(entityType); xmlWriter.WriteAttributeString("id", recordingMbid);
                    xmlWriter.WriteStartElement("user-tag-list");

                    foreach (string tagString in tag.Split(';'))
                    {
                        Debug.WriteLine("tagString: "+tagString);
                        string tagReplaced = MusicBrainzDataProcessor.FindReplaceTag(tagString);
                        if (tagReplaced != "{remove}")
                        {
                            xmlWriter.WriteStartElement("user-tag");
                            if (!Settings.Default.tagSubmitIsDestructive)
                            {
                                xmlWriter.WriteAttributeString("vote", "upvote");
                            }
                            xmlWriter.WriteElementString("name", tagReplaced);
                            xmlWriter.WriteEndElement();
                        }
                    }
                    xmlWriter.WriteEndElement(); // ends user-tag-list XML
                    xmlWriter.WriteEndElement(); // ends individual recording XML
                }
                xmlWriter.WriteEndElement(); // ends entity list XML
            }

            xmlWriter.WriteEndElement(); // ends metadata XML
            xmlWriter.Flush();
            string xmlData = stringWriter.ToString();
            return xmlData;
        }

    }
}

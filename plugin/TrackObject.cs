using System;
using System.IO;
using System.Collections.Generic;
using static MusicBeePlugin.Plugin;
using System.Diagnostics;
using System.Linq;
using plugin.Properties;

namespace plugin
{
    public class MusicBeeTrack
    {

        public string MusicBrainzTrackId { get; private set; }
        public string MusicBrainzReleaseId { get; private set; }
        public string MusicBrainzReleaseGroupId { get; private set; }
        public string[] MusicBrainzArtistId { get; private set; }
        public string FilePath { get; private set; }

        // ratings
        public string Rating { get; private set; }
        public string AlbumRating { get; private set; }


        // for debug and troubleshooting purposes
        public string Title { get; private set; }
        public string Artist { get; private set; }
        public string Album { get; private set; }

        // throwing my own UnsupportedFormatException here instead of using TagLib#'s UnsupportedFormat exception for other files to use.
        // Having TagLib# inside Plugin.cs results in clashes over the TextBox control for some reason.
        public class UnsupportedFormatException : Exception
        {
            public UnsupportedFormatException(string message) : base(message) { }
        }

        private static readonly HashSet<String> bannedExtensions = new HashSet<String> { ".tak", ".midi", ".mid", ".xm", ".uxm", ".mod" };

        public MusicBeeTrack(string path)
        {
            string fileExt = Path.GetExtension(path);
            if (bannedExtensions.Contains(fileExt))
            {
                throw new UnsupportedFormatException($"{fileExt} files are not currently supported by this plugin.");
            }
            else
            {
                var TagLib_TrackData = TagLib.File.Create(path);

                Album = mbApiInterface.Library_GetFileTag(path, MetaDataType.Album);
                // Debug data - not going to be used but useful for debugging purposes.
                Title = mbApiInterface.Library_GetFileTag(path, MetaDataType.TrackTitle);
                Artist = mbApiInterface.Library_GetFileTag(path, MetaDataType.Artist);
                // Rating data - MusicBee seems to output them as strings.
                Rating = mbApiInterface.Library_GetFileTag(path, MetaDataType.Rating);
                AlbumRating = mbApiInterface.Library_GetFileTag(path, MetaDataType.RatingAlbum);
                FilePath = path;

                // MusicBrainz tags: single
                MusicBrainzTrackId = TagLib_TrackData.Tag.MusicBrainzTrackId;
                MusicBrainzReleaseId = TagLib_TrackData.Tag.MusicBrainzReleaseId;
                MusicBrainzReleaseGroupId = TagLib_TrackData.Tag.MusicBrainzReleaseGroupId;

                // MusicBrainz tags: multi
                if (!string.IsNullOrEmpty(TagLib_TrackData.Tag.MusicBrainzArtistId))
                {
                    // TagLib#, instead of returning an array or list, returns strings - the separator seemingly depends on the tag format it's read.
                    MusicBrainzArtistId = TagLib_TrackData.Tag.MusicBrainzArtistId.Split(new Char[] { ';', '/', ',' });
                    for (int i = 0; i < MusicBrainzArtistId.Length; i++)
                    {
                        MusicBrainzArtistId[i] = MusicBrainzArtistId[i].Trim();
                    }
                }

            }

        }

        // for the tag functions, we want to get a collection of tags, as defined in the tag binding settings.
        // it doesn't make as much sense to have these as properties at the moment.
        public List<string> GetAllTagsFromFile(string type = "recording")
        {
            // get the tags the user has chosen to submit
            List<string> tagsToSearch = new List<string>();
            Debug.WriteLine(tagsToSearch.Count);
            List<string> tagList = new List<string>();

            if (type == "recording" || !Settings.Default.separateTagBindings)
            {
                if (!string.IsNullOrEmpty(Settings.Default.recordingTagBindings))
                {
                    tagsToSearch = Settings.Default.recordingTagBindings.Split(';').ToList();
                }
            }
            else
            {
                switch (type)
                {
                    case "release":
                        if (!string.IsNullOrEmpty(Settings.Default.releaseTagBindings))
                        {
                            tagsToSearch = Settings.Default.releaseTagBindings.Split(';').ToList();
                        }
                        break;
                    case "release-group":
                        if (!string.IsNullOrEmpty(Settings.Default.releaseGroupTagBindings))
                        {
                            tagsToSearch = Settings.Default.releaseGroupTagBindings.Split(';').ToList();
                        }   
                        break;
                }
            }
                
            Debug.WriteLine("tagsToSearch: " + string.Join(", ", tagsToSearch));
            if (tagsToSearch.Count > 0)
            {
                foreach (string tag in tagsToSearch)
                {
                    // get tag from finding its key in the dictionary then add to get track tags.
                    MetaDataType tagType = listTagBindings[tag];
                    // clean up tags
                    List<string> tagValue = mbApiInterface.Library_GetFileTag(FilePath, tagType).Split(';').ToList();
                    foreach (string tagString in tagValue)
                    {
                        // do not try to add tags if tagString is empty, otherwise it will add empty strings.
                        if (!string.IsNullOrEmpty(tagString))
                        {
                            string trimmedTag = tagString.Trim();
                            if (!tagList.Contains(trimmedTag))
                            {
                                tagList.Add(trimmedTag);
                            }
                        }
                    }

                }
                if (tagList.Count > 0)
                {
                    return tagList;
                }
                else
                {
                    return null;
                }
                
            }
            else
            {
                return null;
            }
                
        }

    }
}

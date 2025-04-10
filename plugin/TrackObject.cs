using System;
using System.IO;
using System.Collections.Generic;
using static MusicBeePlugin.Plugin;
using System.Diagnostics;
using System.Linq;

namespace plugin
{
    internal class MusicBeeTrack
    {


        public string MusicBrainzTrackId { get; private set; }
        public string MusicBrainzReleaseId { get; private set; }
        public string MusicBrainzReleaseGroupId { get; private set; }
        public string[] MusicBrainzArtistId { get; private set; }
        public string FilePath { get; private set; }

        // ratings
        public string Rating { get; private set; }
        public string AlbumRating { get; private set; }

        // standard tags musicbrainz may be looking for
        public List<string> Genres { get; private set; }
        public List<string> Mood { get; private set; }
        public List<string> Occasion { get; private set; }
        public List<string> Keywords { get; private set; }
       
        // custom tags
        public List<string> Custom1 { get; private set; }
        public List<string> Custom2 { get; private set; }
        public List<string> Custom3 { get; private set; }
        public List<string> Custom4 { get; private set; }
        public List<string> Custom5 { get; private set; }
        public List<string> Custom6 { get; private set; }
        public List<string> Custom7 { get; private set; }
        public List<string> Custom8 { get; private set; }
        public List<string> Custom9 { get; private set; }
        public List<string> Custom10 { get; private set; }
        public List<string> Custom11 { get; private set; }
        public List<string> Custom12 { get; private set; }
        public List<string> Custom13 { get; private set; }
        public List<string> Custom14 { get; private set; }
        public List<string> Custom15 { get; private set; }
        public List<string> Custom16 { get; private set; }


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

        private List<string> GetTagsToList(string path, MetaDataType tag)
        {
            List<string> tags = new List<string>();
            // MusicBee splits multi-value tags with a semicolon, so we need to split them and trim them.
            // Todo: see if this works with AAC. AAC can be really funky across the board.
            foreach (string tagString in mbApiInterface.Library_GetFileTag(path, tag).Split(';').ToList())
            {
                string trimmedTag = tagString.Trim();
                tags.Add(trimmedTag);
            }
            return tags;
        }

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
                // Standard tags
                Genres = GetTagsToList(path, MetaDataType.Genre);
                Mood = GetTagsToList(path, MetaDataType.Mood);
                Occasion = GetTagsToList(path, MetaDataType.Occasion);
                Keywords = GetTagsToList(path, MetaDataType.Keywords);
                // Custom tags
                Custom1 = GetTagsToList(path, MetaDataType.Custom1);
                Custom2 = GetTagsToList(path, MetaDataType.Custom2);
                Custom3 = GetTagsToList(path, MetaDataType.Custom3);
                Custom4 = GetTagsToList(path, MetaDataType.Custom4);
                Custom5 = GetTagsToList(path, MetaDataType.Custom5);
                Custom6 = GetTagsToList(path, MetaDataType.Custom6);
                Custom7 = GetTagsToList(path, MetaDataType.Custom7);
                Custom8 = GetTagsToList(path, MetaDataType.Custom8);
                Custom9 = GetTagsToList(path, MetaDataType.Custom9);
                Custom10 = GetTagsToList(path, MetaDataType.Custom10);
                Custom11 = GetTagsToList(path, MetaDataType.Custom11);
                Custom12 = GetTagsToList(path, MetaDataType.Custom12);
                Custom13 = GetTagsToList(path, MetaDataType.Custom13);
                Custom14 = GetTagsToList(path, MetaDataType.Custom14);
                Custom15 = GetTagsToList(path, MetaDataType.Custom15);
                Custom16 = GetTagsToList(path, MetaDataType.Custom16);

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

    }
}

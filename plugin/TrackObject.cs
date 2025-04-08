using System;
using System.IO;
using static MusicBeePlugin.Plugin;

namespace plugin
{
    internal class MusicBeeTrack
    {

        public string Rating { get; private set; }
        public string AlbumRating { get; private set; }
        public string MusicBrainzTrackId { get; private set; }
        public string MusicBrainzReleaseId { get; private set; }
        public string MusicBrainzReleaseGroupId { get; private set; }
        public string[] MusicBrainzArtistId { get; private set; }
        public string FilePath { get; private set; }

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

        public MusicBeeTrack(string path)
        {
            System.Diagnostics.Debug.WriteLine(path);
            System.Diagnostics.Debug.WriteLine(Path.GetExtension(path));
            
            if (Path.GetExtension(path) == ".tak")
            {
                throw new UnsupportedFormatException("TAK files are not currently supported by this plugin.");
            }
            else
            {
                var TagLib_TrackData = TagLib.File.Create(path);

                // Rating data - MusicBee seems to output them as strings.
                Rating = mbApiInterface.Library_GetFileTag(path, MetaDataType.Rating);
                AlbumRating = mbApiInterface.Library_GetFileTag(path, MetaDataType.RatingAlbum);
                // Debug data - not going to be used but useful for debugging purposes.
                Title = mbApiInterface.Library_GetFileTag(path, MetaDataType.TrackTitle);
                Artist = mbApiInterface.Library_GetFileTag(path, MetaDataType.Artist);
                Album = mbApiInterface.Library_GetFileTag(path, MetaDataType.Album);

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

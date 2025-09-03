using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plugin
{
    // MusicBrainz object classes
    // Used for JSON deserialization of MusicBrainz API responses
    // As the plugin does more, this is going to be expanded.

    /// <summary>
    /// Object representing a MusicBrainz release group.
    /// </summary>
    public class ReleaseGroup
    {


        /// <summary>
        /// A release group's MusicBrainz ID (MBID).
        /// </summary>
        [JsonProperty("id")]
        public string MBID { get; private set; }

        /// <summary>
        /// A release group's title.
        /// </summary>
        [JsonProperty("title")]
        // release title
        public string Title { get; private set; }

        /// <summary>
        /// A release group's disambiguation comment.<br/>
        /// Used to differentiate between release groups with the same name (e.g. Weezer (Blue Album), Weezer (Green Album)).
        /// </summary>
        [JsonProperty("disambiguation")]
        // disambiguation, used as a comment for release groups with the same name (e.g. Weezer (Blue Album), Weezer (Green Album))
        // can be an empty string
        public string Disambiguation { get; private set; }

        /// <summary>
        /// A release group's primary type. (e.g. Album, Single, EP).
        /// </summary>
        [JsonProperty("primary-type")]
        // primary type, always filled in.
        public string PrimaryType { get; private set; }



        private List<string> _secondaryTypes = new List<string>();
        // secondary types, can be an empty array
        /// <summary>
        /// A release group's secondary types. (e.g. Compilation, Soundtrack).<br/>
        /// This is always a list, but can be empty.
        /// </summary>
        [JsonProperty("secondary-types")]
        public List<string> SecondaryTypes
        {
            get => _secondaryTypes;
            private set
            {
            // make sure they're set to lowercase
                _secondaryTypes = new List<string>();
                foreach(var type in value)
                {
                    _secondaryTypes.Add(type.ToLower());
                }

            }
        }

        /// <summary>
        /// A release group's first release date, based on the earliest release in the group.
        /// </summary>
        [JsonProperty("first-release-date")]
        // TODO change to DateTime
        public string FirstReleaseDate { get; private set; }

        [JsonProperty("user-rating")]
        public UserRatingContainer userRatingContainer { get; private set; }

        /// <summary>
        /// A release group's rating from the currently authenticated user.
        /// </summary>
        [JsonIgnore]
        public double? UserRating => userRatingContainer?.Value;

        /// <summary>
        /// A release group's rating from all MusicBrainz users who have rated it.
        /// </summary>
        [JsonProperty("rating")]
        public CommunityRating CommunityRating { get; private set; }

        /// <summary>
        /// A release group's tags from all MusicBrainz users who have applied them.
        /// </summary>
        [JsonProperty("tags")]
        public List<MusicBrainzTag> Tags { get; private set; }

        /// <summary>
        /// A release group's tags from the currently authenticated user.
        /// </summary>
        [JsonProperty("user-tags")]
        public List<MusicBrainzTag> UserTags { get; private set; }

        // TODO add top tags method?

    }

    /// <summary>
    /// Object representing a MusicBrainz release.
    /// </summary>
    public class Release
    {
        /// <summary>
        /// A release's MusicBrainz ID (MBID).
        /// </summary>
        [JsonProperty("id")]
        // MBID for a release
        public string MBID { get; set; }

        /// <summary>
        /// A release's title.
        /// </summary>
        [JsonProperty("title")]
        // release title
        public string Title { get; set; }

        [JsonProperty("date")]
        // TODO change to DateTime
        public string Date { get; set; }

        /// <summary>
        /// A release's barcode, can be null.
        /// </summary>
        [JsonProperty("barcode")]
        // barcode, can be null
        public string Barcode { get; set; }

        /// <summary>
        /// A release's conutry code, can be null.
        /// </summary>
        [JsonProperty("country")]
        // country, can be null
        public string Country { get; set; }

        /// <summary>
        /// A release's disambiguation comment, used for differentiating between releases with the same name (e.g. physical properties like vinyl weight/colour, different editions).
        /// </summary>
        [JsonProperty("disambiguation")]
        // disambiguation, used as a comment for releases with the same name
        public string Disambiguation { get; set; }

        [JsonProperty("status")]
        // status, can be null
        public string Status { get; set; }

        /// <summary>
        /// A release's media (discs/LPs/cassettes/etc).
        /// </summary>
        [JsonProperty("media")]
        public List<ReleaseMedia> Media { get; set; }


    }

    /// <summary>
    /// Object representing the medium of a MusicBrainz release.
    /// </summary>
    public class ReleaseMedia
    {
        /// <summary>
        /// A release medium's format (e.g. CD, Vinyl, Cassette).<br/>
        /// </summary>
        [JsonProperty("format")]
        // format, can be null
        public string Format { get; set; }

        /// <summary>
        /// The number of tracks on this medium.
        /// </summary>
        [JsonProperty("track-count")]
        // number of tracks on this medium
        public int TrackCount { get; set; }

        /// <summary>
        /// The list of tracks on this medium.
        /// </summary>
        [JsonProperty("tracks")]
        public List<ReleaseMediaTrack> Tracks { get; set; }

        /// <summary>
        /// The release medium's title, can be null.
        /// </summary>
        [JsonProperty("title")]
        // title, can be null
        public string Title { get; set; }

        /// <summary>
        /// The position of this medium in the release, if the order matters for your use case.
        /// </summary>
        [JsonProperty("position")]
        // position of this medium in the release (1, 2, etc)
        public int Position { get; set; }

    }

    /// <summary>
    /// Object representing the track inside the medium of a MusicBrainz release.
    /// </summary>
    public class ReleaseMediaTrack
    {
        /// <summary>
        /// The track's MusicBrainz ID (MBID).
        /// </summary>
        [JsonProperty("id")]
        // MBID for a track
        public string MBID { get; set; }

        /// <summary>
        /// The track's title. Can be different from the recording title.
        /// </summary>
        [JsonProperty("title")]
        // track title
        public string Title { get; set; }

        /// <summary>
        /// The track's length in milliseconds. Can be null.
        /// </summary>
        [JsonProperty("length")]
        // length in milliseconds, can be null
        public int? Length { get; set; }

        /// <summary>
        /// The track's position on the medium, if the order matters for your use case.
        /// </summary>
        [JsonProperty("position")]
        // position of this track on the medium (1, 2, etc)
        public int Position { get; set; }

        /// <summary>
        /// The recording associated with this track, can be used for user data like ratings and tags.
        /// </summary>
        [JsonProperty("recording")]
        public Recording Recording { get; set; }
    }

    /// <summary>
    /// Object representing a MusicBrainz recording.
    /// </summary>
    public class Recording
    {
        /// <summary>
        /// The recording's MusicBrainz ID (MBID).
        /// </summary>
        [JsonProperty("id")]
        // MBID for a recording
        public string MBID { get; set; }

        /// <summary>
        /// The recording's title. Tracks on releases can have different titles to the recording.
        /// </summary>
        [JsonProperty("title")]
        // recording title
        public string Title { get; set; }

        /// <summary>
        /// The track's length in milliseconds, typically based on the tracks it's associated with. Can be null.
        /// </summary>
        [JsonProperty("length")]
        // length in milliseconds, can be null
        public int? Length { get; set; }

        /// <summary>
        /// A recording's disambiguation comment.<br/>
        /// Can be useful for differentiating between recordings with the same name.
        /// </summary>
        [JsonProperty("disambiguation")]
        // disambiguation, used as a comment for recordings with the same name
        public string Disambiguation { get; set; }

        /// <summary>
        /// The earliest release date for a recording, based on the releases it's associated with.
        /// </summary>
        [JsonProperty("first-release-date")]
        // TODO change to DateTime
        public string FirstReleaseDate { get; set; }


        /// <summary>
        /// The recording's rating from the currently authenticated user.
        /// </summary>
        [JsonProperty("user-rating")]
        public UserRatingContainer userRatingContainer { get; set; }
        [JsonIgnore]
        public double? CurrentUserRating => userRatingContainer?.Value;

        /// <summary>
        /// The recording's rating from all MusicBrainz users who have rated it.
        /// </summary>
        [JsonProperty("rating")]
        public CommunityRating CommunityRating { get; set; }

    }


    public class UserRatingContainer
    {
        // User ratings are placed in a dictionary with a "value" key.
        [JsonProperty("value")]
        public double? Value { get; set; }

    }

    public class CommunityRating
    {
        /// <summary>
        /// The average rating value of the community rating, from 0.0 to 5.0.
        /// </summary>
        [JsonProperty("rating")]
        public double Rating { get; set; }

        /// <summary>
        /// The amount of votes that contributed to the community rating.
        /// </summary>
        [JsonProperty("votes-count")]
        public int VotesCount { get; set; }
    }

    public class MusicBrainzTag
    {   
        /// <summary>
        /// The name of the tag.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The count of how many times this tag has been applied by the community.
        /// </summary>
        [JsonProperty("count")]
        public int? Count { get; set; }
    }
}

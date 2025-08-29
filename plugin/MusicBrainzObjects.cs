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
    // As the plugin does more, this should be expanded.

    public class ReleaseGroup
    {
        [JsonProperty("id")]
        // MBID for a release
        public string MBID { get; set; }

        [JsonProperty("title")]
        // release title
        public string Title { get; set; }

        [JsonProperty("disambiguation")]
        // disambiguation, used as a comment for release groups with the same name (e.g. Weezer (Blue Album), Weezer (Green Album))
        // can be an empty string
        public string Disambiguation { get; set; }

        [JsonProperty("primary-type")]
        // primary type, always filled in.
        public string PrimaryType { get; set; }

        
        private List<string> _secondaryTypes = new List<string>();
        // secondary types, can be an empty array
        [JsonProperty("secondary-types")]
        public List<string> SecondaryTypes
        {
            get => _secondaryTypes;
            set {
            // make sure they're set to lowercase
                _secondaryTypes = new List<string>();
                foreach(var type in value)
                {
                    _secondaryTypes.Add(type.ToLower());
                }

            }
        }

        [JsonProperty("first-release-date")]
        // TODO change to DateTime
        public string FirstReleaseDate { get; set; }

        [JsonProperty("user-rating")]
        public UserRatingContainer userRatingContainer { get; set; }

        [JsonIgnore]
        public double? UserRating => userRatingContainer?.Value;

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
        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("votes-count")]
        public int VotesCount { get; set; }
    }
}

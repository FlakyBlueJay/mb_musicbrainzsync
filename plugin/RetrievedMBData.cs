using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plugin
{
    /// <summary>
    /// A class for storing data retrieved from the MusicBrainz API.
    /// </summary>
    public class RetrievedMBData
    {
        // The key is the entity type (typically "recording", "release-group" or "release")
        // The value is a Dictionary, with its keys being MusicBrainz IDs and rating/tag values.

        public Dictionary<string, Dictionary<string, float>> Ratings { get; set; } = new Dictionary<string, Dictionary<string, float>>();

        /// <summary>
        /// Tags are stored as values in the value dictionary, as a List of strings. This is so that tags can be properly merged during the retrieval process.
        /// </summary>
        public Dictionary<string, Dictionary<string, List<string>>> Tags { get; set; } = new Dictionary<string, Dictionary<string, List<string>>>();

        public Dictionary<string, Dictionary<string, List<string>>> Genres { get; set; } = new Dictionary<string, Dictionary<string, List<string>>>();
    }
}

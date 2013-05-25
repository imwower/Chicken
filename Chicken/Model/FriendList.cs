using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chicken.Model
{
    public class FriendList
    {
        [JsonProperty("users")]
        public IList<UserProfile> Users { get; set; }

        [JsonProperty("next_cursor")]
        public string NextCursor { get; set; }

        [JsonProperty("previous_cursor")]
        public string PreviousCursor { get; set; }
    }
}

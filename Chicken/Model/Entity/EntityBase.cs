using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chicken.Model.Entity
{
    public class Entities
    {
        [JsonProperty("media")]
        public List<Media> Medias { get; set; }

        [JsonProperty("hashtags")]
        public List<HashTag> HashTags { get; set; }

        [JsonProperty("urls")]
        public List<UrlEntity> Urls { get; set; }

        [JsonProperty("user_mentions")]
        public List<UserMention> UserMentions { get; set; }
    }

    public class EntityBase
    {
        [JsonProperty("indices")]
        public List<int> Indices { get; set; }

        public int Index
        {
            get
            {
                return Indices[0];
            }
        }

        public int Length
        {
            get
            {
                return Indices[1] - Indices[0];
            }
        }
    }
}

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
        public virtual EntityType EntityType { get; private set; }

        public virtual string Text { get; set; }

        //[JsonProperty("indices")]
        //public List<int> Indices { get; set; }

        public int Index { get; set; }
    }

    public enum EntityType
    {
        None = 0,
        Media = 1,
        HashTag = 2,
        Url = 3,
        UserMention = 4,
    }
}

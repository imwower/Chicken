using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chicken.Model.Entity
{
    public class Entities
    {
        [JsonProperty("media")]
        public List<MediaEntity> Medias { get; set; }

        [JsonProperty("hashtags")]
        public List<HashTag> HashTags { get; set; }

        [JsonProperty("urls")]
        public List<UrlEntity> Urls { get; set; }

        [JsonProperty("user_mentions")]
        public List<UserMention> UserMentions { get; set; }
    }

    public class EntityBase
    {
        public virtual EntityType EntityType { get; set; }

        public int Index { get; set; }

        public virtual string Text { get; set; }

        #region for user mention
        public virtual string Id { get; set; }
        public virtual string DisplayName { get; set; }
        #endregion

        #region for hashtag
        public virtual string DisplayText { get; set; }
        #endregion

        #region for url
        public virtual string DisplayUrl { get; set; }
        public virtual string ExpandedUrl { get; set; }
        //public virtual string TruncatedUrl { get; set; }
        [JsonIgnore]
        public virtual string TruncatedUrl
        {
            get
            {
                int index = DisplayUrl.IndexOf("/");
                if (index != -1)
                    return "[" + DisplayUrl.Remove(index) + "]";
                else
                    return "[" + DisplayUrl + "]";
            }
            set
            { }
        }
        #endregion

        #region for media
        public virtual string MediaUrl { get; set; }
        #endregion
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

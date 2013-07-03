using Newtonsoft.Json;

namespace Chicken.Model.Entity
{
    public class UrlEntity : EntityBase
    {
        public override EntityType EntityType
        {
            get
            {
                return EntityType.Url;
            }
        }

        [JsonProperty("url")]
        public override string Text { get; set; }

        [JsonProperty("display_url")]
        public string DisplayUrl { get; set; }

        [JsonProperty("expanded_url")]
        public string ExpandedUrl { get; set; }

        [JsonIgnore]
        public string TruncatedUrl
        {
            get
            {
                int index = DisplayUrl.IndexOf("/");
                if (index != -1)
                    return "[" + DisplayUrl.Remove(index) + "]";
                else
                    return "[" + DisplayUrl + "]";
            }
        }
    }
}

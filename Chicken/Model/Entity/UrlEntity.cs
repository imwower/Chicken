using Newtonsoft.Json;

namespace Chicken.Model.Entity
{
    public class UrlEntity : EntityBase
    {
        [JsonProperty("url")]
        public string UrlString { get; set; }

        [JsonProperty("display_url")]
        public string DisplayUrl { get; set; }

        [JsonProperty("expanded_url")]
        public string ExpandedUrl { get; set; }

        public string TruncatedUrl
        {
            get
            {
                return DisplayUrl.Remove(DisplayUrl.IndexOf("/" + 1));
            }
        }
    }
}

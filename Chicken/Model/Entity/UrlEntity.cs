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
        public override string DisplayUrl { get; set; }

        [JsonProperty("expanded_url")]
        public override string ExpandedUrl { get; set; }
    }
}

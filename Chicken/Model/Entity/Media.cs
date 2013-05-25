using Newtonsoft.Json;

namespace Chicken.Model.Entity
{
    public class Media
    {
        public string Id { get; set; }

        public string Type { get; set; }

        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }
    }
}

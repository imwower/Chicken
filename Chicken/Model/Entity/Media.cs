using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chicken.Model.Entity
{
    public class Media
    {
        [JsonProperty("id_str")]
        public string Id { get; set; }

        public string Type { get; set; }

        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }
    }
}

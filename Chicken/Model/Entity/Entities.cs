
using Newtonsoft.Json;
using System.Collections.Generic;
namespace Chicken.Model.Entity
{
    public class Entities
    {
        [JsonProperty("media")]
        public Media[] Medias { get; set; }

        [JsonProperty("hashtags")]
        public HashTag[] HashTags { get; set; }
    }
}

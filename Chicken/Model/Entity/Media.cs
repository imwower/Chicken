using Newtonsoft.Json;

namespace Chicken.Model.Entity
{
    public class Media : EntityBase
    {
        [JsonProperty("id_str")]
        public string Id { get; set; }

        public string Type { get; set; }

        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }

        public string MediaUrlThumb
        {
            get
            {
                return MediaUrl + ":thumb";
            }
        }

        public string MediaUrlSmall
        {
            get
            {
                return MediaUrl + ":small";
            }
        }
    }
}

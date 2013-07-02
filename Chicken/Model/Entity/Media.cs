using Newtonsoft.Json;

namespace Chicken.Model.Entity
{
    public class MediaEntity : UrlEntity
    {
        public override EntityType EntityType
        {
            get
            {
                return EntityType.Media;
            }
        }

        [JsonProperty("id_str")]
        public override string Id { get; set; }

        //[JsonProperty("type")]
        //public override string MediaType { get; set; }

        [JsonProperty("media_url_https")]
        public override string MediaUrl { get; set; }

        //[JsonIgnore]
        //public string MediaUrlThumb
        //{
        //    get
        //    {
        //        return MediaUrl + ":thumb";
        //    }
        //}

        [JsonIgnore]
        public string MediaUrlSmall
        {
            get
            {
                return MediaUrl + ":small";
            }
        }
    }
}

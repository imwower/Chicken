using System;
using Newtonsoft.Json;

namespace Chicken.Model
{
    public class TweetConfiguration : ModelBase
    {
        public DateTime LastUpdateTime { get; set; }

        [JsonProperty("short_url_length")]
        public int MaxShortUrlLength { get; set; }

        [JsonProperty("short_url_length_https")]
        public int MaxShortUrlLengthHttps { get; set; }
    }
}

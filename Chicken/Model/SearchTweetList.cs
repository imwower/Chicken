using Newtonsoft.Json;

namespace Chicken.Model
{
    public class SearchTweetList : ModelBase
    {
        [JsonProperty("statuses")]
        public TweetList Statuses { get; set; }
    }
}

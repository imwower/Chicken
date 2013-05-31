using Newtonsoft.Json;

namespace Chicken.Model
{
    public class Tweet : TweetBase
    {
        [JsonProperty("retweeted_status")]
        public Retweet RetweetStatus { get; set; }
    }

    public class TweetList<T> : ModelBaseList<T> where T : Tweet
    {
    }
}

using Chicken.Model.Entity;
using Newtonsoft.Json;

namespace Chicken.Model
{
    public class TweetBase : ModelBase
    {
        public User User { get; set; }

        [JsonProperty("created_at")]
        public string CreatedDate { get; set; }

        [JsonProperty("id_str")]
        public string Id { get; set; }

        public string Text { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("retweet_count")]
        public string RetweetCount { get; set; }

        [JsonProperty("favorite_count")]
        public string FavouriteCount { get; set; }

        [JsonProperty("entities")]
        public Entities Entities { get; set; }

        [JsonProperty("favorited")]
        public bool Favourited { get; set; }

        [JsonProperty("retweeted")]
        public bool Retweeted { get; set; }

        [JsonProperty("in_reply_to_status_id")]
        public string InReplyToTweetId { get; set; }

        [JsonProperty("in_reply_to_user_id")]
        public string InReplayToUserId { get; set; }

        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }
    }
}

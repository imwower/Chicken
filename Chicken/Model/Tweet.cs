using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Chicken.Common;
using Chicken.Model.Entity;

namespace Chicken.Model
{
    public class Tweet
    {
        public User User { get; set; }

        [JsonProperty("created_at")]
        public string CreatedDate { get; set; }

        public string Id { get; set; }

        public string Text { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("retweet_count")]
        public string RetweetCount { get; set; }

        [JsonProperty("favorite_count")]
        public string FavouriteCount { get; set; }

        public Entities Entities { get; set; }

        [JsonProperty("favorited")]
        public bool Favourited { get; set; }

        [JsonProperty("retweeted")]
        public bool Retweeted { get; set; }

        [JsonProperty("in_reply_to_status_id")]
        public string InReplayToTweetId { get; set; }

        [JsonProperty("in_reply_to_user_id")]
        public string InReplayToUserId { get; set; }

        [JsonProperty("geo")]
        public string Geo { get; set; }
    }
}

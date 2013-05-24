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

namespace Chicken.Model
{
    public class UserProfile : User
    {
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("profile_banner_url")]
        public string UserProfileBannerImage { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Url { get; set; }

        [JsonProperty("statuses_count")]
        public string TweetsCount { get; set; }

        [JsonProperty("friends_count")]
        public string FollowingCount { get; set; }

        [JsonProperty("followers_count")]
        public string FollowersCount { get; set; }

        [JsonProperty("favourites_count")]
        public string FavouritesCount { get; set; }
    }
}

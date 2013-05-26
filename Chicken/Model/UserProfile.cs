using Newtonsoft.Json;

namespace Chicken.Model
{
    public class UserProfile : User
    {
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

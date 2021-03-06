﻿using System.Collections.Generic;
using Chicken.Model.Entity;
using Newtonsoft.Json;

namespace Chicken.Model
{
    public class UserProfileDetail : UserProfile
    {
        [JsonProperty("profile_banner_url")]
        public string UserProfileBannerImage { get; set; }

        public string Location { get; set; }

        public string Url { get; set; }

        [JsonProperty("entities")]
        public UserProfileEntities UserProfileEntities { get; set; }

        [JsonProperty("statuses_count")]
        public string TweetsCount { get; set; }

        [JsonProperty("friends_count")]
        public string FollowingCount { get; set; }

        [JsonProperty("followers_count")]
        public string FollowersCount { get; set; }

        [JsonProperty("favourites_count")]
        public string FavoritesCount { get; set; }
    }

    public class UserProfileEntities
    {
        [JsonProperty("url")]
        public UserProfileUrlEntities UserProfileUrlEntities { get; set; }

        [JsonProperty("description")]
        public UserProfileDescriptionEntities DescriptionEntities { get; set; }
    }

    public class UserProfileUrlEntities
    {
        [JsonProperty("urls")]
        public List<UrlEntity> Urls { get; set; }
    }

    public class UserProfileDescriptionEntities
    {
        [JsonProperty("urls")]
        public List<UrlEntity> Urls { get; set; }
    }
}

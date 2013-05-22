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
    public class UserProfile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImage { get; set; }

        [JsonProperty("profile_banner_url")]
        public string UserProfileBannerImage { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Url { get; set; }
    }
}

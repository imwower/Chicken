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

namespace Chicken.Model
{
    public class Tweet
    {
        public User User { get; set; }

        public string Text { get; set; }

        [JsonProperty("created_at")]
        public string CreatedDate { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        public long Id { get; set; }
    }

    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImage { get; set; }
    }
}

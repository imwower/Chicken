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
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chicken.Model
{
    public class FriendList
    {
        [JsonProperty("users")]
        public List<UserProfile> Users { get; set; }

        [JsonProperty("next_cursor")]
        public string NextCursor { get; set; }

        [JsonProperty("previous_cursor")]
        public string PreviousCursor { get; set; }
    }
}

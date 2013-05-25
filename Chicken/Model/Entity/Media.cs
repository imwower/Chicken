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

namespace Chicken.Model.Entity
{
    public class Media
    {
        public string Id { get; set; }

        public string Type { get; set; }

        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }
    }
}

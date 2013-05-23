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
    public class DirectMessage
    {  
        public string Id { get; set; }

        public string Text { get; set; }

        public User Sender { get; set; }

        [JsonProperty("created_at")]
        public string CreatedDate { get; set; }
    }
}

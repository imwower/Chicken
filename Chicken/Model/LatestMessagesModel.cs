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

namespace Chicken.Model
{
    public class LatestMessagesModel
    {
        public string SinceId { get; set; }

        public string MaxId { get; set; }

        public string SinceIdByMe { get; set; }

        public string MaxIdByMe { get; set; }

        public Dictionary<string, Conversation> Messages { get; set; }

        public LatestMessagesModel()
        {
            Messages = new Dictionary<string, Conversation>();
        }
    }

    public class Conversation
    {
        public User User { get; set; }

        public List<DirectMessage> Messages { get; set; }

        public Conversation()
        {
            Messages = new List<DirectMessage>();
        }
    }
}

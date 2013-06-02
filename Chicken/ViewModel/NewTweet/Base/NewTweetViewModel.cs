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

namespace Chicken.ViewModel.NewTweet.Base
{
    public class NewTweetViewModel : NotificationObject
    {
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                RaisePropertyChanged("Text");
            }
        }

        public string InReplyToTweetId { get; set; }
        public string InReplyToUserId { get; set; }
    }
}

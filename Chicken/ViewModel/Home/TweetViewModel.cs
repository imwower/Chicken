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
using Chicken.Model;
using Chicken.Common;
using System.Diagnostics;

namespace Chicken.ViewModel.Home
{
    public class TweetViewModel
    {
        private Tweet tweet;

        private UserViewModel user;
        public UserViewModel User
        {
            get
            {
                return user;
            }
        }

        public TweetViewModel(Tweet tweet)
        {
            this.tweet = tweet;
            this.user = new UserViewModel(tweet.User);
        }

        public string Text
        {
            get
            {
                return tweet.Text;
            }
        }

        public string CreatedDate
        {
            get
            {
                return tweet.CreatedDate.ParseToDateTime();
            }
        }

        public string Source
        {
            get
            {
                return tweet.Source.ParseToSource();
            }
        }

        public Uri SourceUrl
        {
            get
            {
                return new Uri(tweet.Source.ParseToSourceUrl(), UriKind.Absolute);
            }
        }

    }
}

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

namespace Chicken.ViewModel
{
    public class TweetViewModel : NotificationObject
    {
        private Tweet tweet;
        private UserViewModel userViewModel;
        public UserViewModel UserViewModel
        {
            get
            {
                return userViewModel;
            }
            set
            {
                userViewModel = value;
                RaisePropertyChanged("UserViewModel");
            }
        }

        public TweetViewModel(Tweet tweet)
        {
            this.tweet = tweet;
            this.UserViewModel = new UserViewModel(tweet.User);
        }
        public string Text
        {
            get
            {
                return tweet.Text;
            }
            set
            {
                tweet.Text = value;
                RaisePropertyChanged("Text");
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

    public class UserViewModel : NotificationObject
    {
        private User user;
        public UserViewModel(User user)
        {
            this.user = user;
        }

        public string Name
        {
            get
            {
                return user.Name;
            }
        }

        public int Id
        {
            get
            {
                return user.Id;
            }
        }

        public string ProfileImage
        {
            get
            {
                return user.ProfileImage;
            }
        }
    }
}

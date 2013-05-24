using System;
using Chicken.Common;
using Chicken.Model;

namespace Chicken.ViewModel.Home.Base
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

        public string Id
        {
            get
            {
                return tweet.Id;
            }
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
                if (tweet.Source.Length == 3 && tweet.Source.Equals("web"))
                {
                    return "web";
                }
                return tweet.Source.ParseToSource();
            }
        }

        public Uri SourceUrl
        {
            get
            {
                if (tweet.Source.Length == 3 && tweet.Source.Equals("web"))
                {
                    return new Uri("https://github.com/", UriKind.Absolute);
                }
                return new Uri(tweet.Source.ParseToSourceUrl(), UriKind.Absolute);
            }
        }
    }
}

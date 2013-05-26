using System;
using Chicken.Common;
using Chicken.Model;

namespace Chicken.ViewModel.Home.Base
{
    /// <summary>
    /// tweet view model is the summary of tweet
    /// use tweet detail view model for more info
    /// </summary>
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
                return TwitterHelper.ParseToDateTime(tweet.CreatedDate);
            }
        }

        public bool IncludeMedia
        {
            get
            {
                return tweet.Entities != null &&
                    tweet.Entities.Medias != null;
            }
        }

        public string RetweetCount
        {
            get
            {
                return tweet.RetweetCount;
            }
        }

        public string Source
        {
            get
            {
                return TwitterHelper.ParseToSource(tweet.Source);
            }
        }

        public Uri SourceUrl
        {
            get
            {
                return new Uri(TwitterHelper.ParseToSourceUrl(tweet.Source), UriKind.Absolute);
            }
        }

        public string InReplayToTweetId
        {
            get
            {
                return tweet.InReplayToTweetId;
            }
        }

        public bool IncludeCoordinates
        {
            get
            {
                return tweet.Coordinates != null;
            }
        }
    }
}

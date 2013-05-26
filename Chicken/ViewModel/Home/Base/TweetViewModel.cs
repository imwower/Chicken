using System;
using Chicken.Common;
using Chicken.Model;
using Chicken.Model.Entity;

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

        public Entities Entities
        {
            get
            {
                return tweet.Entities;
            }
        }

        public bool Retweeted
        {
            get
            {
                return tweet.Retweeted;
            }
        }

        public bool Favourited
        {
            get
            {
                return tweet.Favourited;
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

        public Coordinates Coordinates
        {
            get
            {
                return tweet.Coordinates;
            }
        }
    }
}

using System;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Status.Base;

namespace Chicken.ViewModel.Home.Base
{
    public class TweetViewModel
    {
        private TweetBase tweet;
        private TweetBase originalTweet;
        private UserViewModel user;
        private EntitiesViewModel entitiesViewModel;
        private CoordinatesViewModel coordinatesViewModel;

        public TweetViewModel(Tweet tweet)
        {
            if (tweet.RetweetStatus != null)
            {
                this.originalTweet = tweet as TweetBase;
                this.tweet = tweet.RetweetStatus;
            }
            else
            {
                this.tweet = tweet as TweetBase;
            }

            if (this.tweet.User != null)
            {
                this.user = new UserViewModel(this.tweet.User);
            }
            if (this.tweet.Entities != null)
            {
                this.entitiesViewModel = new EntitiesViewModel(this.tweet.Entities);
            }
            if (this.tweet.Coordinates != null)
            {
                this.coordinatesViewModel = new CoordinatesViewModel(this.tweet.Coordinates);
            }
        }

        public bool IncludeRetweet
        {
            get
            {
                return this.originalTweet != null;
            }
        }

        public TweetBase OriginalTweet
        {
            get
            {
                return originalTweet;
            }
        }

        public UserViewModel User
        {
            get
            {
                return user;
            }
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
                return entitiesViewModel != null
                    && entitiesViewModel.MediasViewModel != null
                    && entitiesViewModel.MediasViewModel.Count != 0;
            }
        }

        public EntitiesViewModel EntitiesViewModel
        {
            get
            {
                return entitiesViewModel;
            }
        }

        public bool Retweeted
        {
            get
            {
                return tweet.Retweeted;
            }
        }

        public bool Favorited
        {
            get
            {
                return tweet.Favorited;
            }
        }

        public string RetweetCount
        {
            get
            {
                return tweet.RetweetCount;
            }
        }

        public string FavoriteCount
        {
            get
            {
                return tweet.FavoriteCount;
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

        public string InReplyToTweetId
        {
            get
            {
                return tweet.InReplyToTweetId;
            }
        }

        public bool IncludeCoordinates
        {
            get
            {
                return coordinatesViewModel != null;
            }
        }

        public CoordinatesViewModel CoordinatesViewModel
        {
            get
            {
                return coordinatesViewModel;
            }
        }

        /// <summary>
        ///show retweet count, favorite count and location panel
        /// </summary>
        public bool NeedShowRetweetIcons
        {
            get
            {
                return RetweetCount != "0" ||
                    FavoriteCount != "0" ||
                    coordinatesViewModel != null;
            }
        }
    }
}

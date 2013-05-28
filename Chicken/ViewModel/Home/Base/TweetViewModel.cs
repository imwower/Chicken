using System;
using Chicken.Common;
using Chicken.Model;
using Chicken.Model.Entity;
using Chicken.ViewModel.Status.Base;

namespace Chicken.ViewModel.Home.Base
{
    public class TweetViewModel
    {
        private Tweet tweet;
        private UserViewModel user;
        private EntitiesViewModel entitiesViewModel;
        private CoordinatesViewModel coordinatesViewModel;

        public TweetViewModel(Tweet tweet)
        {
            this.tweet = tweet;
            if (tweet.User != null)
            {
                this.user = new UserViewModel(tweet.User);
            }
            if (tweet.Entities != null)
            {
                this.entitiesViewModel = new EntitiesViewModel(tweet.Entities);
            }
            if (tweet.Coordinates != null)
            {
                this.coordinatesViewModel = new CoordinatesViewModel(tweet.Coordinates);
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

        public string FavouriteCount
        {
            get
            {
                return tweet.FavouriteCount;
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
        ///show retweet count, favourite count and location panel
        /// </summary>
        public bool NeedShowRetweetIcons
        {
            get
            {
                return RetweetCount != "0" ||
                    FavouriteCount != "0" ||
                    coordinatesViewModel != null;
            }
        }
    }
}

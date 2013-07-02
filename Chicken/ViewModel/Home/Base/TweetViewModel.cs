using System;
using Chicken.Common;
using Chicken.Model;
using Chicken.Model.Entity;

namespace Chicken.ViewModel.Home.Base
{
    public class TweetViewModel
    {
        #region private
        private TweetBase tweet;
        private TweetBase originalTweet;
        #endregion

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
        }

        public bool IncludeRetweet
        {
            get
            {
                return this.originalTweet != null;
            }
        }

        public TweetBase Tweet
        {
            get
            {
                return tweet;
            }
        }

        public TweetBase OriginalTweet
        {
            get
            {
                return originalTweet;
            }
        }

        public User User
        {
            get
            {
                return tweet.User;
            }
        }

        public bool IsSentByMe
        {
            get
            {
                return tweet.IsSentByMe;
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
                return this.tweet.Entities != null &&
                    this.tweet.Entities.Medias != null &&
                    this.tweet.Entities.Medias.Count != 0;
            }
        }

        public Entities Entities
        {
            get
            {
                return this.tweet.Entities;
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
                return this.tweet.Coordinates != null;
            }
        }

        public Coordinates Coordinates
        {
            get
            {
                return this.tweet.Coordinates;
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
                    IncludeCoordinates;
            }
        }
    }
}

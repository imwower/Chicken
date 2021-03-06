﻿using System;
using Chicken.Common;
using Chicken.Model;
using Chicken.Model.Entity;

namespace Chicken.ViewModel.Base
{
    public class TweetViewModel : LazyDataItem
    {
        #region private
        protected TweetBase tweet;
        protected UserViewModel user;
        protected TweetViewModel originalTweet;
        #endregion

        public TweetViewModel(Tweet data)
        {
            #region retweet
            if (data.RetweetStatus != null)
            {
                //for retweet,
                //get the real Id for max_id/since_id
                //while loading/refreshing
                string id = data.RetweetStatus.Id;
                this.tweet = data.RetweetStatus;
                this.tweet.Id = data.Id;
                data.RetweetStatus = null;
                data.Id = id;
                this.originalTweet = new TweetViewModel(data);
            }
            else
            {
                this.tweet = data;
            }
            #endregion
            this.user = new UserViewModel(this.tweet.User);
        }

        public TweetBase Tweet
        {
            get
            {
                return tweet;
            }
        }

        public TweetViewModel OriginalTweet
        {
            get
            {
                return originalTweet;
            }
        }

        public virtual string Id
        {
            get
            {
                return tweet.Id;
            }
        }

        public string CreatedDate
        {
            get
            {
                return TwitterHelper.ParseToDateTime(tweet.CreatedDate);
            }
        }

        public bool IsSentByMe
        {
            get
            {
                return tweet.IsSentByMe;
            }
        }

        public UserViewModel User
        {
            get
            {
                return user;
            }
        }

        public string Text
        {
            get
            {
                return tweet.Text;
            }
        }

        public bool IncludeRetweet
        {
            get
            {
                return this.originalTweet != null;
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

        public bool IsRetweeted
        {
            get
            {
                return tweet.Retweeted;
            }
        }

        public bool IsFavorited
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

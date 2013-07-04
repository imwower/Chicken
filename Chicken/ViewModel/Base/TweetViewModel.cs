using System;
using Chicken.Common;
using Chicken.Controls;
using Chicken.Model;
using Chicken.Model.Entity;

namespace Chicken.ViewModel.Base
{
    public class TweetViewModel : NotificationObject, ILazyDataItem
    {
        #region private
        private TweetBase tweet;
        private UserViewModel user;
        private TweetViewModel originalTweet;
        private bool isPaused;
        private LazyDataLoadState currentState;
        #endregion

        public TweetViewModel(Tweet data)
        {
            #region tweet
            if (data.RetweetStatus != null)
            {
                this.tweet = data.RetweetStatus;
                this.originalTweet = new TweetViewModel(data.RetweetStatus);
            }
            else
            {
                this.tweet = data;
            }
            #endregion
            this.user = new UserViewModel(this.tweet.User);
        }

        public TweetViewModel(TweetBase retweet)
        {
            this.tweet = retweet;
        }

        public TweetBase Tweet
        {
            get
            {
                return tweet;
            }
        }

        public virtual string Id
        {
            get
            {
                return tweet.Id;
            }
        }

        protected virtual string createdDate
        {
            get
            {
                return TwitterHelper.ParseToDateTime(tweet.CreatedDate);
            }
        }

        public string CreatedDate
        {
            get
            {
                return createdDate;
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

        public TweetViewModel OriginalTweet
        {
            get
            {
                return originalTweet;
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

        public void GoToState(LazyDataLoadState state)
        {
            currentState = state;
            switch (state)
            {
                case LazyDataLoadState.Minimum:
                case LazyDataLoadState.Cached:
                case LazyDataLoadState.Reloading:
                case LazyDataLoadState.Loading:
                    if (User != null)
                        User.ChangeVisibility(true);
                    break;
                case LazyDataLoadState.Unloaded:
                    if (User != null)
                        User.ChangeVisibility(false);
                    break;
                case LazyDataLoadState.Loaded:
                    break;
            }
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Unpause()
        {
            isPaused = false;
        }

        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
        }

        public LazyDataLoadState CurrentState
        {
            get
            {
                return currentState;
            }
        }

        public event EventHandler<LazyDataItemStateChangedEventArgs> CurrentStateChanged;

        public event EventHandler<LazyDataItemPausedStateChangedEventArgs> PauseStateChanged;
    }
}

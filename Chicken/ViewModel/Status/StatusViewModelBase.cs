using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Status
{
    public class StatusViewModelBase : PivotItemViewModelBase
    {
        #region properties
        private TweetDetailViewModel tweet;
        public TweetDetailViewModel Tweet
        {
            get
            {
                return tweet;
            }
            set
            {
                if (tweet != value)
                {
                    tweet = value;
                    RaisePropertyChanged("Tweet");
                }
            }
        }
        protected string nextCursor = "-1";
        protected string previousCursor = string.Empty;
        public ObservableCollection<UserProfileViewModel> UserList { get; set; }
        #endregion

        public StatusViewModelBase()
        {
        }

        #region public methods
        public virtual void AddFavorite()
        {
            if (IsLoading)
                return;
            IsLoading = true;
            var action = Tweet.IsFavorited ? AddToFavoriteActionType.Destroy : AddToFavoriteActionType.Create;
            TweetService.AddToFavorites(Tweet.Id, action,
               data =>
               {
                   #region handle error
                   if (data.HasError)
                   {
                       IsLoading = false;
                       return;
                   }
                   #endregion
                   string message = Tweet.IsFavorited ?
                      LanguageHelper.GetString("Toast_Msg_RemoveFavoriteSuccessfully") :
                       LanguageHelper.GetString("Toast_Msg_AddFavoriteSuccessfully");
                   App.HandleMessage(new ToastMessage
                   {
                       Message = message
                   });
                   Tweet = new TweetDetailViewModel(data);
                   Refresh();
               });
        }

        public virtual void Retweet()
        {
            if (IsLoading)
                return;
            if (Tweet.User.IsPrivate)
            {
                App.HandleMessage(new ToastMessage
                {
                    Message = LanguageHelper.GetString("Toast_Msg_CannotRetweetPrivateTweet"),
                });
                return;
            }
            if (!Tweet.IsRetweeted)
            {
                IsLoading = true;
                var action = RetweetActionType.Create;
                //get the original tweet id
                string statusId = Tweet.OriginalTweet == null ? Tweet.Id : Tweet.OriginalTweet.Id;
                TweetService.Retweet(statusId, action,
                   data =>
                   {
                       #region handle error
                       if (data.HasError)
                       {
                           IsLoading = false;
                           return;
                       }
                       #endregion
                       App.HandleMessage(new ToastMessage
                       {
                           Message = LanguageHelper.GetString("Toast_Msg_RetweetSuccessfully"),
                       });
                       IsolatedStorageService.CreateObject(Const.StatusPage_StatusDetail, data);
                       Refresh();
                   });
            }
        }

        public virtual void Reply()
        {
            DoAction(NewTweetActionType.Reply);
        }

        public virtual void Quote()
        {
            DoAction(NewTweetActionType.Quote);
        }

        public virtual void Delete()
        {
            if (IsLoading ||
                !Tweet.IsSentByMe)
                return;
            IsLoading = true;
            TweetService.DeleteTweet(Tweet.Id,
                data =>
                {
                    #region handle error
                    if (data.HasError)
                    {
                        IsLoading = false;
                        return;
                    }
                    App.HandleMessage(new ToastMessage
                    {
                        Message = LanguageHelper.GetString("Toast_Msg_DeleteSuccessfully"),
                        Complete = () => NavigationServiceManager.NavigateTo(Const.HomePage)
                    });
                    #endregion
                });
        }
        #endregion

        #region protected methods
        protected bool CheckIfLoaded()
        {
            var tweet = IsolatedStorageService.GetObject<Tweet>(Const.StatusPage_StatusDetail);
            if (tweet == null)
            {
                base.Refreshed();
                return false;
            }
            Tweet = new TweetDetailViewModel(tweet);
            return true;
        }

        #region for retweets and favorites pivot
        protected void RefreshUserProfiles(string userIds)
        {
            TweetService.GetUserProfiles(userIds,
                userProfiles =>
                {
                    if (!userProfiles.HasError)
                    {
                        for (int i = userProfiles.Count - 1; i >= 0; i--)
                            UserList.Insert(0, new UserProfileViewModel(userProfiles[i]));
                    }
                    base.Refreshed();
                });
        }

        protected void LoadUserProfiles(string userIds)
        {
            TweetService.GetUserProfiles(userIds,
                userProfiles =>
                {
                    if (!userProfiles.HasError)
                    {
                        foreach (var userProfile in userProfiles)
                            UserList.Add(new UserProfileViewModel(userProfile));
                    }
                    base.Loaded();
                });
        }
        #endregion
        #endregion

        #region private
        private void DoAction(NewTweetActionType type)
        {
            if (IsLoading)
                return;
            IsLoading = false;
            NewTweetModel newTweet = new NewTweetModel
            {
                Type = type,
            };
            switch (type)
            {
                case NewTweetActionType.Quote:
                    newTweet.Text = Const.QUOTECHARACTER + " " + Tweet.User.DisplayName + " " + Tweet.Text;
                    newTweet.InReplyToStatusId = Tweet.Id;
                    newTweet.InReplyToUserScreenName = Tweet.User.DisplayName;
                    break;
                case NewTweetActionType.Reply:
                    newTweet.Text = Tweet.User.DisplayName + " ";
                    newTweet.InReplyToStatusId = Tweet.Id;
                    newTweet.InReplyToUserScreenName = Tweet.User.DisplayName;
                    break;
                case NewTweetActionType.PostNew:
                case NewTweetActionType.None:
                default:
                    break;
            }
            NavigationServiceManager.NavigateTo(Const.NewTweetPage, newTweet);
        }
        #endregion
    }
}

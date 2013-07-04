using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Status
{
    public class StatusViewModelBase : PivotItemViewModelBase
    {
        #region properties
        private TweetViewModel tweet;
        public TweetViewModel Tweet
        {
            get
            {
                return tweet;
            }
            set
            {
                tweet = value;
                RaisePropertyChanged("Tweet");
            }
        }
        protected string nextCursor = "-1";
        protected string previousCursor = string.Empty;
        private ObservableCollection<UserProfile> userList;
        public ObservableCollection<UserProfile> UserList
        {
            get
            {
                return userList;
            }
            set
            {
                userList = value;
                RaisePropertyChanged("UserList");
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public StatusViewModelBase()
        {
            ClickHandler = this.ClickAction;
            ItemClickHandler = this.ItemClickAction;
        }

        #region public methods
        public virtual void AddFavorite()
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;
            var action = Tweet.IsFavorited ? AddToFavoriteActionType.Destroy : AddToFavoriteActionType.Create;
            TweetService.AddToFavorites<Tweet>(Tweet.Id, action,
                t =>
                {
                    var toastMessage = new ToastMessage();
                    List<ErrorMessage> errors = t.Errors;
                    #region handle error
                    if (errors != null && errors.Count != 0)
                    {
                        IsLoading = false;
                        toastMessage.Message = errors[0].Message;
                        HandleMessage(toastMessage);
                        return;
                    }
                    #endregion
                    toastMessage.Message = Tweet.IsFavorited ? "Remove favorites successfully" : "Add to favorites successfully";
                    HandleMessage(toastMessage);
                    Tweet = new TweetViewModel(t);
                    Refresh();
                });
        }

        public virtual void Retweet()
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;
            var action = RetweetActionType.Create;
            if (!Tweet.IsRetweeted)
            {
                TweetService.Retweet<Tweet>(Tweet.Id, action,
                   t =>
                   {
                       var toastMessage = new ToastMessage();
                       List<ErrorMessage> errors = t.Errors;
                       #region handle error
                       if (errors != null && errors.Count != 0)
                       {
                           IsLoading = false;
                           toastMessage.Message = errors[0].Message;
                           HandleMessage(toastMessage);
                           return;
                       }
                       #endregion
                       toastMessage.Message = "Retweet successfully";
                       HandleMessage(toastMessage);
                       Tweet = new TweetViewModel(t);
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
            if (Tweet.IsSentByMe)
            {
                //TODO: delete tweet
            }
        }
        #endregion

        #region actions
        private void ClickAction(object parameter)
        {
            NavigationServiceManager.NavigateTo(PageNameEnum.ProfilePage, parameter);
        }

        private void ItemClickAction(object parameter)
        {
            NavigationServiceManager.NavigateTo(PageNameEnum.StatusPage, parameter);
        }
        #endregion

        #region protected methods
        #region for retweets and favorites pivot
        protected void RefreshUserProfiles(string userIds)
        {
            TweetService.GetUserProfiles<UserProfileList<UserProfile>>(userIds,
                userProfiles =>
                {
                    for (int i = userProfiles.Count - 1; i >= 0; i--)
                    {
                        UserList.Insert(0, userProfiles[i]);
                    }
                    base.Refreshed();
                });
        }

        protected void LoadUserProfiles(string userIds)
        {
            TweetService.GetUserProfiles<UserProfileList<UserProfile>>(userIds,
                userProfiles =>
                {
                    foreach (var userProfile in userProfiles)
                    {
                        UserList.Add(userProfile);
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
            {
                return;
            }
            NewTweetModel newTweet = new NewTweetModel
            {
                Type = type,
            };
            switch (type)
            {
                case NewTweetActionType.Quote:
                    newTweet.Text = Const.QUOTECHARACTER + " " + Tweet.User.ScreenName + " " + Tweet.Text;
                    newTweet.InReplyToStatusId = Tweet.Id;
                    newTweet.InReplyToUserScreenName = Tweet.User.ScreenName;
                    break;
                case NewTweetActionType.Reply:
                    newTweet.Text = Tweet.User.ScreenName + " ";
                    newTweet.InReplyToStatusId = Tweet.Id;
                    newTweet.InReplyToUserScreenName = Tweet.User.ScreenName;
                    break;
                case NewTweetActionType.PostNew:
                case NewTweetActionType.None:
                default:
                    break;
            }
            IsLoading = false;
            NavigationServiceManager.NavigateTo(PageNameEnum.NewTweetPage, newTweet);
        }
        #endregion
    }
}

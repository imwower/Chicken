using System.Collections.ObjectModel;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Profile.Base;

namespace Chicken.ViewModel.Status
{
    public class StatusViewModelBase : PivotItemViewModelBase
    {
        #region event handler
        protected ClickEventHandler AddFavoriteHandler;
        protected ClickEventHandler RetweetHandler;
        protected ClickEventHandler ReplyHandler;
        protected ClickEventHandler QuoteHandler;
        #endregion

        #region properties
        private string statusId;
        public string StatusId
        {
            get
            {
                return statusId;
            }
            set
            {
                statusId = value;
                RaisePropertyChanged("StatusId");
            }
        }

        protected string nextCursor = "-1";
        protected string previousCursor;
        private ObservableCollection<UserProfileViewModel> userList;
        public ObservableCollection<UserProfileViewModel> UserList
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
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        #region public methods
        public virtual void AddFavorite(object parameter)
        {
            if (AddFavoriteHandler == null)
            {
                return;
            }
            AddFavoriteHandler(parameter);
        }

        public virtual void Retweet(object parameter)
        {
            if (RetweetHandler == null)
            {
                return;
            }
            RetweetHandler(parameter);
        }

        public virtual void Reply(object parameter)
        {
            if (ReplyHandler == null)
            {
                return;
            }
            ReplyHandler(parameter);
        }

        public virtual void Quote(object parameter)
        {
            if (QuoteHandler == null)
            {
                return;
            }
            QuoteHandler(parameter);
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
                        UserList.Insert(0, new UserProfileViewModel(userProfiles[i]));
                    }
                });
        }

        protected void LoadUserProfiles(string userIds)
        {
            TweetService.GetUserProfiles<UserProfileList<UserProfile>>(userIds,
                userProfiles =>
                {
                    foreach (var userProfile in userProfiles)
                    {
                        UserList.Add(new UserProfileViewModel(userProfile));
                    }
                });
        }
        #endregion
        #endregion
    }
}

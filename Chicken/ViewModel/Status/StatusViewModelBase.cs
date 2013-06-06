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
        protected LoadEventHandler AddFavoriteHandler;
        protected LoadEventHandler RetweetHandler;
        protected LoadEventHandler ReplyHandler;
        protected LoadEventHandler QuoteHandler;
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
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        #region public methods
        public virtual void AddFavorite()
        {
            if (AddFavoriteHandler == null)
            {
                return;
            }
            AddFavoriteHandler();
        }

        public virtual void Retweet()
        {
            if (RetweetHandler == null)
            {
                return;
            }
            RetweetHandler();
        }

        public virtual void Reply()
        {
            if (ReplyHandler == null)
            {
                return;
            }
            ReplyHandler();
        }

        public virtual void Quote()
        {
            if (QuoteHandler == null)
            {
                return;
            }
            QuoteHandler();
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
                        UserList.Add(new UserProfileViewModel(userProfile));
                    }
                    base.Loaded();
                });
        }
        #endregion
        #endregion
    }
}

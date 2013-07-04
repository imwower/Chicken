using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModelBase : PivotItemViewModelBase
    {
        #region event handler
        public delegate void NewMessageEventHandler();
        public NewMessageEventHandler NewMessageHandler;
        #endregion

        #region properties
        private UserProfileDetailViewModel userProfile;
        public UserProfileDetailViewModel UserProfile
        {
            get
            {
                return userProfile;
            }
            set
            {
                if (userProfile != value)
                {
                    userProfile = value;
                    RaisePropertyChanged("UserProfile");
                }
            }
        }

        #region for tweets and favorites pivot item
        private ObservableCollection<TweetViewModel> tweetList;
        public ObservableCollection<TweetViewModel> TweetList
        {
            get
            {
                return tweetList;
            }
            set
            {
                tweetList = value;
                RaisePropertyChanged("TweetList");
            }
        }
        #endregion
        #region for following and followers pivot item
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
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public ProfileViewModelBase()
        {
            ClickHandler = this.ClickAction;
        }

        #region public method
        public virtual void NewMessage()
        {
            if (IsLoading)
            {
                return;
            }
            if (NewMessageHandler != null)
            {
                IsLoading = false;
                NewMessageHandler();
            }
        }

        public virtual void Mention()
        {
            if (IsLoading)
            {
                return;
            }
            NewTweetModel newTweet = new NewTweetModel
            {
                Type = NewTweetActionType.Mention,
                Text = UserProfile.DisplayName + " ",
            };
            IsLoading = false;
            NavigationServiceManager.NavigateTo(PageNameEnum.NewTweetPage, newTweet);
        }

        public virtual void EditMyProfile()
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(PageNameEnum.EditMyProfilePage);
        }
        #endregion

        #region actions
        private void ClickAction(object parameter)
        {
            var user = parameter as User;
            if (UserProfile.Id == user.Id)
            {
                NavigationServiceManager.ChangeSelectedIndex((int)ProfilePageEnum.ProfileDetail);
            }
            else
            {
                NavigationServiceManager.NavigateTo(PageNameEnum.ProfilePage, user);
            }
        }
        #endregion

        #region protected methods
        protected bool CheckIfFollowingPrivate()
        {
            if (this.UserProfile.IsPrivate &&
                !this.UserProfile.IsFollowing)
            {
                HandleMessage(new ToastMessage
                {
                    Message = "you must follow the private user first"
                });
                return false;
            }
            return true;
        }
        #region for following and followers pivot
        protected void RefreshUserProfiles(string userIds)
        {
            TweetService.GetUserProfiles<UserProfileList>(userIds,
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
            TweetService.GetUserProfiles<UserProfileList>(userIds,
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

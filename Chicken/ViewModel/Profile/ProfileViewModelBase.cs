using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModelBase : PivotItemViewModelBase
    {
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
        public ObservableCollection<TweetViewModel> TweetList { get; set; }
        #endregion
        #region for following and followers pivot item
        protected string nextCursor = "-1";
        protected string previousCursor;
        public ObservableCollection<UserProfileViewModel> UserList { get; set; }
        #endregion
        #endregion

        public ProfileViewModelBase()
        {
            ClickHandler = this.ClickAction;
        }

        #region public method
        public virtual void NewMessage()
        {
        }

        public virtual void Mention()
        {
            if (IsLoading)
                return;
            IsLoading = false;
            NewTweetModel newTweet = new NewTweetModel
            {
                Type = NewTweetActionType.Mention,
                InReplyToUserScreenName = UserProfile.DisplayName,
                Text = UserProfile.DisplayName + " ",
            };
            NavigationServiceManager.NavigateTo(Const.NewTweetPage, newTweet);
        }

        public virtual void EditMyProfile()
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(Const.EditMyProfilePage);
        }
        #endregion

        #region actions
        private void ClickAction(object parameter)
        {
            IsLoading = false;
            var user = parameter as User;
            if (UserProfile.Id == user.Id)
                NavigationServiceManager.ChangeSelectedIndex((int)ProfilePageEnum.ProfileDetail);
            else
                NavigationServiceManager.NavigateTo(Const.ProfilePage, user);
        }
        #endregion

        #region protected methods

        protected bool CheckIfFollowingPrivate()
        {
            var profile = IsolatedStorageService.GetObject<UserProfileDetail>(Const.ProfilePage_UserProfileDetail);
            if (profile == null)
            {
                base.Refreshed();
                return false;
            }
            UserProfile = new UserProfileDetailViewModel(profile);
            if (!profile.IsMyself &&
                UserProfile.IsPrivate &&
                !UserProfile.IsFollowing)
            {
                App.HandleMessage(new ToastMessage
                {
                    Message = LanguageHelper.GetString("Toast_Msg_NotFollowPrivateUser", UserProfile.DisplayName),
                });
                return false;
            }
            return true;
        }

        #region for following and followers pivot
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
    }
}

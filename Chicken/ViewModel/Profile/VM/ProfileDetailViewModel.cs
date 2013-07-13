using System.Collections.Generic;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Profile.VM
{
    public class ProfileDetailViewModel : ProfileViewModelBase
    {
        #region properties
        private bool followedBy;
        public bool FollowedBy
        {
            get
            {
                return followedBy;
            }
            set
            {
                followedBy = value;
                RaisePropertyChanged("FollowedBy");
            }
        }
        #endregion

        public ProfileDetailViewModel()
        {
            RefreshHandler = this.RefreshAction;
        }

        #region actions
        private void RefreshAction()
        {
            var profile = IsolatedStorageService.GetObject<UserProfileDetail>(Const.ProfilePage_UserProfileDetail);
            if (profile == null)
            {
                IsLoading = false;
                return;
            }
            UserProfile = new UserProfileDetailViewModel(profile);
            if (UserProfile.IsMyself)
            {
                IsolatedStorageService.CreateAuthenticatedUser(profile);
                App.InitAuthenticatedUser();
                base.Refreshed();
                return;
            }
            GetFollowedByState();
        }
        #endregion

        #region override
        public override void NewMessage()
        {
            if (IsLoading)
                return;
            if (!followedBy)
            {
                App.HandleMessage(new ToastMessage
                {
                    Message = LanguageHelper.GetString("Toast_Msg_NotFollowYou", UserProfile.ScreenName),
                });
                return;
            }
            IsLoading = false;
            var newMessage = new NewMessageModel
            {
                User = UserProfile.User,
            };
            NavigationServiceManager.NavigateTo(Const.NewMessagePage, newMessage);
        }
        #endregion

        #region private
        private void GetFollowedByState()
        {
            TweetService.GetFriendshipConnections(this.UserProfile.Id,
                friendships =>
                {
                    if (!friendships.HasError && friendships.Count != 0)
                    {
                        List<string> connections = friendships[0].Connections;
                        if (connections.Contains(Const.FOLLOWED_BY))
                            FollowedBy = true;
                    }
                    base.Refreshed();
                });
        }
        #endregion
    }
}

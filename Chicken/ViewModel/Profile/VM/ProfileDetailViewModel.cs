using System.Collections.Generic;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;

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
            if (UserProfile.IsMyself)
            {
                IsolatedStorageService.CreateAuthenticatedUser(UserProfile.UserProfileDetail);
                App.InitAuthenticatedUser();
            }
            else
            {
                GetFollowedByState();
            }
            base.Refreshed();
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
                    Message = UserProfile.ScreenName + " did not follow you"
                });
                return;
            }
            IsLoading = false;
            var newMessage = new NewMessageModel
            {
                User = UserProfile.User,
            };
            NavigationServiceManager.NavigateTo(PageNameEnum.NewMessagePage, newMessage);
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
                });
        }
        #endregion
    }
}

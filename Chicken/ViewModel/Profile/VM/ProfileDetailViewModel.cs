using System.Collections.Generic;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;

namespace Chicken.ViewModel.Profile.VM
{
    public class ProfileDetailViewModel : ProfileViewModelBase
    {
        #region properties
        private UserProfileDetail userProfileViewModel;
        public UserProfileDetail UserProfileViewModel
        {
            get
            {
                return userProfileViewModel;
            }
            set
            {
                userProfileViewModel = value;
                RaisePropertyChanged("UserProfileViewModel");
            }
        }
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
            Header = "Profile";
            RefreshHandler = this.RefreshAction;
            NewMessageHandler = this.NewMessageAction;
        }

        #region actions
        private void RefreshAction()
        {
            TweetService.GetUserProfileDetail<UserProfileDetail>(UserProfile,
                userProfileDetail =>
                {
                    this.UserProfileViewModel = userProfileDetail;
                    if (UserProfile.IsMyself)
                    {
                        IsolatedStorageService.CreateAuthenticatedUser(userProfileDetail);
                        App.InitAuthenticatedUser();
                    }
                    else
                    {
                        GetFollowedByState();
                    }
                    base.Refreshed();
                });
        }

        private void NewMessageAction()
        {
            if (!followedBy)
            {
                HandleMessage(new ToastMessage
                {
                    Message = userProfileViewModel.ScreenName + " did not follow you"
                });
                return;
            }
            var newMessage = new NewMessageModel
            {
                User = userProfileViewModel as User,
            };
            NavigationServiceManager.NavigateTo(PageNameEnum.NewMessagePage, newMessage);
        }
        #endregion

        #region private
        private void GetFollowedByState()
        {
            TweetService.GetFriendshipConnections<Friendships<Friendship>>(this.UserProfileViewModel.Id,
                friendships =>
                {
                    if (friendships != null && friendships.Count != 0)
                    {
                        Friendship friendship = friendships[0];
                        List<string> connections = friendship.Connections;
                        if (connections.Contains(Const.FOLLOWED_BY))
                        {
                            FollowedBy = true;
                        }
                    }
                });
        }
        #endregion
    }
}

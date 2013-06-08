using System.Collections.Generic;
using Chicken.Common;
using Chicken.Model;

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
            RefreshHandler = RefreshAction;
        }

        private void RefreshAction()
        {
            TweetService.GetUserProfileDetail<UserProfileDetail>(User.Id,
                userProfileDetail =>
                {
                    this.UserProfileViewModel = userProfileDetail;
                    GetFollowedByState();
                    base.Refreshed();
                });
        }

        private void GetFollowedByState()
        {
            TweetService.GetFriendshipConnections<Friendships<Friendship>>(User.Id,
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
    }
}

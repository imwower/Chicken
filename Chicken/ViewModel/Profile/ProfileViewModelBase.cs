using Chicken.Service;
using Chicken.ViewModel.Profile.Base;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModelBase : ViewModelBase
    {
        #region properties
        private string userId;
        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                RaisePropertyChanged("UserId");
            }
        }

        private FriendListViewModel friendList;
        public FriendListViewModel FriendList
        {
            get
            {
                return friendList;
            }
            set
            {
                friendList = value;
                RaisePropertyChanged("FriendList");
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion
    }
}

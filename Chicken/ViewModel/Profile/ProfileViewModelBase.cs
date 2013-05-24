using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using Chicken.Service;
using System.Collections.ObjectModel;

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

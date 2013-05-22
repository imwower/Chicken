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
using Chicken.Service;

namespace Chicken.ViewModel.Profile
{
    public class ProfilePivotViewModel : ProfileViewModelBase
    {
        UserProfileViewModel userProfileViewModel;
        public UserProfileViewModel UserProfileViewModel
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

        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        public ProfilePivotViewModel()
        {
            Header = "Profile";
        }

        public override void Refresh()
        {
            var userProfile = TweetService.GetUserProfile(UserId);
            this.UserProfileViewModel = new Profile.UserProfileViewModel(userProfile);

            base.Refresh();
        }
    }
}

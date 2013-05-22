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
using System.Windows.Navigation;
using Chicken.Service;
using Microsoft.Phone.Controls;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModel : NavigationViewModelBase
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

        public ProfileViewModel()
        {

        }

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            string id = NavigationContext.QueryString["id"];
            var userProfile = TweetService.GetUserProfile(id);
            this.UserProfileViewModel = new UserProfileViewModel(userProfile);

        }

        public void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
 
        }
    }
}

using System;
using System.Windows.Navigation;
using Chicken.Common;
using Chicken.ViewModel.Profile;
using Microsoft.Phone.Controls;

namespace Chicken.View
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        ProfileViewModel profileViewModel;

        public ProfilePage()
        {
            InitializeComponent();
            profileViewModel = new ProfileViewModel();
            this.MainPivot.DataContext = profileViewModel;
            this.ProfilePivotItem.DataContext = profileViewModel.PivotItems[0];
            this.UserTweetsPivotItem.DataContext = profileViewModel.PivotItems[1];
            this.UserFollowingPivotItem.DataContext = profileViewModel.PivotItems[2];
            this.UserFollowersPivotItem.DataContext = profileViewModel.PivotItems[3];
            this.UserFavouritesPivotItem.DataContext = profileViewModel.PivotItems[4];

            this.MainPivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(MainPivot_LoadedPivotItem);
        }

        private void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            profileViewModel.MainPivot_LoadedPivotItem();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string userId = NavigationContext.QueryString[TwitterHelper.USER_ID];
            profileViewModel.OnNavigatedTo(userId);
        }
    }
}
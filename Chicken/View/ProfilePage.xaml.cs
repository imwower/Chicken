using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Chicken.Common;
using Chicken.Service.Interface;
using Chicken.ViewModel.Profile;
using Microsoft.Phone.Controls;

namespace Chicken.View
{
    public partial class ProfilePage : PhoneApplicationPage, INavigationService
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
            int selectedIndex = (sender as Pivot).SelectedIndex;
            Dispatcher.BeginInvoke(() =>
                {
                    profileViewModel.MainPivot_LoadedPivotItem(selectedIndex);
                });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string userId = NavigationContext.QueryString[Const.USER_ID];
            profileViewModel.OnNavigatedTo(userId);
        }

        public void ChangeSelectedIndex(int selectedIndex, IDictionary<string, object> parameters = null)
        {
            this.MainPivot.SelectedIndex = selectedIndex;
        }
    }
}
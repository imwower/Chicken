using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using Chicken.ViewModel;
using Chicken.ViewModel.Profile;
using Chicken.Common;

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

            this.MainPivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(MainPivot_LoadedPivotItem);
        }

        private void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                profileViewModel.MainPivot_LoadedPivotItem(sender, e);
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string userId = NavigationContext.QueryString[TwitterHelper.USER_ID];
            profileViewModel.OnNavigatedTo(userId);
        }
    }
}
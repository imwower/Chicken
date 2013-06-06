using System;
using System.Windows.Navigation;
using Chicken.Common;
using Chicken.Service;
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
            string userId = IsolatedStorageService.GetAndDeleteObject<string>(Const.PageNameEnum.ProfilePage);
            profileViewModel.UserId = userId;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            IsolatedStorageService.CreateObject(Const.PageNameEnum.ProfilePage, profileViewModel.UserId);
        }

        public void ChangeSelectedIndex(int selectedIndex)
        {
            this.MainPivot.SelectedIndex = selectedIndex;
        }
    }
}
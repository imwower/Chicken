using System.Windows.Navigation;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel;
using Chicken.ViewModel.Profile;
using Microsoft.Phone.Controls;

namespace Chicken.View
{
    public partial class ProfilePage : PivotPageBase
    {
        protected override Pivot Pivot
        {
            get
            {
                return this.MainPivot;
            }
        }
        private ProfileViewModel profileViewModel;
        protected override PivotViewModelBase PivotViewModelBase
        {
            get
            {
                return this.profileViewModel;
            }
        }

        public ProfilePage()
        {
            InitializeComponent();
            profileViewModel = new ProfileViewModel();
            this.MainPivot.DataContext = profileViewModel;
            base.Init();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var user = IsolatedStorageService.GetAndDeleteObject<User>(Const.PageNameEnum.ProfilePage);
            profileViewModel.User = user;
        }
    }
}
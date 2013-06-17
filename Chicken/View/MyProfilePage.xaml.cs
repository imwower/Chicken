
using Chicken.ViewModel.Profile;
using Chicken.ViewModel;
using System.Windows.Navigation;
using Chicken.Service;
namespace Chicken.View
{
    public partial class MyProfilePage : PivotPageBase
    {
        protected override Microsoft.Phone.Controls.Pivot Pivot
        {
            get
            {
                return this.MainPivot;
            }
        }
        private MyProfileViewModel myProfileViewModel;
        protected override PivotViewModelBase PivotViewModelBase
        {
            get
            {
                return this.myProfileViewModel;
            }
        }

        public MyProfilePage()
        {
            InitializeComponent();
            myProfileViewModel = new MyProfileViewModel();
            this.MainPivot.DataContext = myProfileViewModel;
            base.Init();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //do not delete data, in case of null error:
            var user = IsolatedStorageService.GetAuthenticatedUser();
            myProfileViewModel.User = user;
        }
    }
}
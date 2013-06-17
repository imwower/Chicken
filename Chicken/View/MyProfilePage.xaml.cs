using System.Windows.Navigation;
using Chicken.Service;
using Chicken.ViewModel;
using Chicken.ViewModel.Profile;

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
    }
}
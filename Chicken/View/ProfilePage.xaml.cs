using Chicken.ViewModel.Profile;

namespace Chicken.View
{
    public partial class ProfilePage : PivotPageBase
    {
        public ProfilePage()
        {
            InitializeComponent();
            Pivot = this.MainPivot;
            PivotViewModelBase = new ProfileViewModel();
            base.Init();
        }
    }
}
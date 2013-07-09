using Chicken.ViewModel.Settings;

namespace Chicken.View
{
    public partial class SettingsPage : PivotPageBase
    {
        public SettingsPage()
        {
            InitializeComponent();
            Pivot = this.MainPivot;
            PivotViewModelBase = new SettingsViewModel();
            base.Init();
        }
    }
}
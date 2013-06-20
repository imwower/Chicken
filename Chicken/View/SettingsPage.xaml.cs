using Chicken.ViewModel;
using Chicken.ViewModel.Settings;
using Microsoft.Phone.Controls;

namespace Chicken.View
{
    public partial class SettingsPage : PivotPageBase
    {
        protected override Pivot Pivot
        {
            get
            {
                return this.MainPivot;
            }
        }
        private SettingsViewModel settingsViewModel;
        protected override PivotViewModelBase PivotViewModelBase
        {
            get
            {
                return this.settingsViewModel;
            }
        }

        public SettingsPage()
        {
            InitializeComponent();
            settingsViewModel = new SettingsViewModel();
            this.DataContext = settingsViewModel;
            base.Init();
        }
    }
}
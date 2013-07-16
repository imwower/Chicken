using System.ComponentModel;
using System.Windows;
using Chicken.ViewModel.Settings;

namespace Chicken.View
{
    public partial class APISettingsPage : PageBase
    {
        #region properties
        private APISettingsViewModel apiSettingsViewModel;
        #endregion

        public APISettingsPage()
        {
            InitializeComponent();
            apiSettingsViewModel = new APISettingsViewModel();
            this.DataContext = apiSettingsViewModel;
            this.Loaded += APISettingsPage_Loaded;
            this.BackKeyPress += APISettingsPage_BackKeyPress;
        }

        #region loaded
        private void APISettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!apiSettingsViewModel.IsInited)
                apiSettingsViewModel.Refresh();
        }

        private void APISettingsPage_BackKeyPress(object sender, CancelEventArgs e)
        {
            if (App.Settings == null || App.Settings.APISettings == null)
                //close app:
                base.Page_OnBackKeyPress(sender, e);
        }
        #endregion
    }
}
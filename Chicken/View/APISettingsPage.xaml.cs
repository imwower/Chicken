using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
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

        #region url text changed
        private void Url_TextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = (sender as TextBox).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
        #endregion
    }
}
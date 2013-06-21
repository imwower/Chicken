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
            apiSettingsViewModel = new APISettingsViewModel()
            {
                ToastMessageHandler = ToastMessageHandler
            };
            this.DataContext = apiSettingsViewModel;
            this.Loaded += APISettingsPage_Loaded;
        }

        private void APISettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!apiSettingsViewModel.IsInited)
            {
                apiSettingsViewModel.Refresh();
            }
        }

        private void Url_TextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = (sender as TextBox).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
    }
}
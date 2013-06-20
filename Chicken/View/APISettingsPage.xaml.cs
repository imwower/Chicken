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
        }

        private void Url_TextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = (sender as TextBox).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
    }
}
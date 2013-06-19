using Chicken.ViewModel.Profile;
using System.Windows;
using System.Windows.Controls;

namespace Chicken.View
{
    public partial class EditMyProfilePage : PageBase
    {
        private MyProfileViewModel myProfileViewModel;

        public EditMyProfilePage()
        {
            InitializeComponent();
            myProfileViewModel = new MyProfileViewModel();
            myProfileViewModel.ToastMessageHandler = ToastMessageHandler;
            this.Loaded += EditMyProfilePage_Loaded;
            this.DataContext = myProfileViewModel;
        }

        void EditMyProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!myProfileViewModel.IsInited)
            {
                myProfileViewModel.Refresh();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            var binding = textbox.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
    }
}
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

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            myProfileViewModel.MyProfile.Name = (sender as TextBox).Text;
        }

        private void Location_TextChanged(object sender, TextChangedEventArgs e)
        {
            myProfileViewModel.MyProfile.Location = (sender as TextBox).Text;
        }

        private void Url_TextChanged(object sender, TextChangedEventArgs e)
        {
            myProfileViewModel.MyProfile.Url = (sender as TextBox).Text;
        }

        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            myProfileViewModel.MyProfile.Description = (sender as TextBox).Text;
        }
    }
}
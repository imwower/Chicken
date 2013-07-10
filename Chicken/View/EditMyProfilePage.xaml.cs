using System.Windows;
using System.Windows.Controls;
using Chicken.ViewModel.Profile;

namespace Chicken.View
{
    public partial class EditMyProfilePage : PageBase
    {
        private EditMyProfileViewModel myProfileViewModel;

        public EditMyProfilePage()
        {
            InitializeComponent();
            myProfileViewModel = new EditMyProfileViewModel();
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
            myProfileViewModel.MyProfile.UserProfileDetail.Name = (sender as TextBox).Text;
        }

        private void Location_TextChanged(object sender, TextChangedEventArgs e)
        {
            myProfileViewModel.MyProfile.UserProfileDetail.Location = (sender as TextBox).Text;
        }

        private void Url_TextChanged(object sender, TextChangedEventArgs e)
        {
            myProfileViewModel.MyProfile.UserProfileDetail.Url = (sender as TextBox).Text;
        }

        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            myProfileViewModel.MyProfile.UserProfileDetail.Text = (sender as TextBox).Text;
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using Chicken.ViewModel.Profile;

namespace Chicken.View
{
    public partial class EditMyProfilePage : PageBase
    {
        #region properties
        private EditMyProfileViewModel myProfileViewModel;
        #endregion

        public EditMyProfilePage()
        {
            InitializeComponent();
            myProfileViewModel = new EditMyProfileViewModel
            {
                BeforeSaveHandler = this.BeforeSaveAction
            };
            this.Loaded += EditMyProfilePage_Loaded;
            this.DataContext = myProfileViewModel;
        }

        #region loaded
        void EditMyProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!myProfileViewModel.IsInited)
            {
                myProfileViewModel.Refresh();
            }
        }
        #endregion

        #region actions
        private void BeforeSaveAction()
        {
            this.Focus();
        }
        #endregion

        #region text changed
        private void Url_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length <= textbox.MaxLength)
                myProfileViewModel.MyProfile.UserProfileDetail.Url = textbox.Text;
        }

        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Text.Length <= textbox.MaxLength)
                myProfileViewModel.MyProfile.UserProfileDetail.Text = textbox.Text;
        }
        #endregion
    }
}
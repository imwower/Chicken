using System.Windows;
using System.Windows.Navigation;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.NewMessage;
using Microsoft.Phone.Controls;

namespace Chicken.View
{
    public partial class NewMessagePage : PhoneApplicationPage
    {
        private NewMessageViewModel newMessageViewModel;

        public NewMessagePage()
        {
            InitializeComponent();
            newMessageViewModel = new NewMessageViewModel();
            this.DataContext = newMessageViewModel;
            this.Loaded += NewMessagePage_Loaded;
        }

        void NewMessagePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!newMessageViewModel.IsInited)
            {
                newMessageViewModel.Refresh();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var user = IsolatedStorageService.GetAndDeleteObject<User>(Const.PageNameEnum.NewMessagePage);
            newMessageViewModel.User = user;
        }
    }
}
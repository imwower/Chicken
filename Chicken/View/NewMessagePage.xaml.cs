using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Chicken.ViewModel.NewMessage;
using System.Windows.Navigation;

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
        }
    }
}
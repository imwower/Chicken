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
using Chicken.ViewModel;
using System.Windows.Navigation;
using Chicken.ViewModel.Home;

namespace Chicken
{
    public partial class MainPage : PhoneApplicationPage
    {
        MainViewModel mainViewModel;

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            this.MainPivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(MainPivot_LoadedPivotItem);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            mainViewModel = new MainViewModel();
            this.MainPivot.DataContext = mainViewModel;
            this.HomePivotItem.DataContext = mainViewModel.PivotItems[0];
            this.MentionsPivotItem.DataContext = mainViewModel.PivotItems[1];
        }

        void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    mainViewModel.MainPivot_LoadedPivotItem(sender, e);
                });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //if (mainViewModel != null)
            //{
            //    mainViewModel.NavigationContext = NavigationContext;
            //    mainViewModel.OnNavigatedTo(e);
            //}
        }
    }
}
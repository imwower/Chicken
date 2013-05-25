using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Chicken.Service.Interface;
using Chicken.ViewModel.Home;
using Microsoft.Phone.Controls;

namespace Chicken
{
    public partial class MainPage : PhoneApplicationPage, INavigationService
    {
        MainViewModel mainViewModel;

        public MainPage()
        {
            InitializeComponent();
            this.MainPivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(MainPivot_LoadedPivotItem);

            mainViewModel = new MainViewModel();
            this.MainPivot.DataContext = mainViewModel;
            this.HomePivotItem.DataContext = mainViewModel.PivotItems[0];
            this.MentionsPivotItem.DataContext = mainViewModel.PivotItems[1];
            this.DMsPivotItem.DataContext = mainViewModel.PivotItems[2];
        }

        void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            int selectedIndex = (sender as Pivot).SelectedIndex;
            Dispatcher.BeginInvoke(() =>
                {
                    mainViewModel.MainPivot_LoadedPivotItem(selectedIndex);
                });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        public void ChangeSelectedIndex(int selectedIndex, IDictionary<string, object> parameters = null)
        {
            this.MainPivot.SelectedIndex = selectedIndex;
        }
    }
}
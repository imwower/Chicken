using System;
using Chicken.Service.Interface;
using Chicken.ViewModel.Home;
using Microsoft.Phone.Controls;

namespace Chicken.View
{
    public partial class MainPage : PhoneApplicationPage, INavigationService
    {
        MainViewModel mainViewModel;

        public MainPage()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel();
            this.MainPivot.DataContext = mainViewModel;
            this.MainPivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(MainPivot_LoadedPivotItem);
        }

        void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            int selectedIndex = (sender as Pivot).SelectedIndex;
            Dispatcher.BeginInvoke(() =>
            {
                mainViewModel.MainPivot_LoadedPivotItem(selectedIndex);
            });
        }

        public void ChangeSelectedIndex(int selectedIndex)
        {
            this.MainPivot.SelectedIndex = selectedIndex;
        }
    }
}
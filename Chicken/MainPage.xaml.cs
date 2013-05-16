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

namespace Chicken
{
    public partial class MainPage : PhoneApplicationPage
    {
        MainViewModel mainViewModel;
        public MainPage()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel();
            this.MainPivot.DataContext = mainViewModel;
            this.MainPivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(MainPivot_LoadedPivotItem);
        }

        private void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    var selectedIndex = MainPivot.SelectedIndex;
                    if (mainViewModel.PivotList[selectedIndex] == null)
                    {
                        switch (selectedIndex)
                        {
                            case 0:
                                mainViewModel.PivotList.Insert(0, new HomeViewModel());
                                break;
                        }
                    }
                });
        }
    }
}
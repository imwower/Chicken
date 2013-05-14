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
        public MainPage()
        {
            InitializeComponent();
            this.MainPivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(MainPivot_LoadedPivotItem);
        }

        private void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    var pivotItem = this.MainPivot.SelectedItem as PivotItem;
                    var baseViewModel = pivotItem.DataContext as BaseViewModel;
                    if (baseViewModel != null)
                    {
                        return;
                    }
                    switch (this.MainPivot.SelectedIndex)
                    {
                        case 0:
                            pivotItem.DataContext = new HomeViewModel(this.HomeListBoxControl);
                            break;
                        case 1:
                            pivotItem.DataContext = new MentionsViewModel(this.MentionsListBoxControl);
                            break;
                        default:
                            break;
                    }
                });
        }
    }
}
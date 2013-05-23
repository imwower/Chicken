using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.Phone.Controls;

namespace Chicken.ViewModel.Home
{
    public class MainViewModel : NavigationViewModelBase
    {
        public MainViewModel()
        {
            var baseViewModelList = new List<ViewModelBase>
            {
                new HomeViewModel(),
                new MentionsViewModel(),
                new DMsPivotViewModel(),
            };
            PivotItems = new ObservableCollection<ViewModelBase>(baseViewModelList);
        }

        public void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            var pivot = sender as Pivot;
            int index = pivot.SelectedIndex;
            if (!PivotItems[index].IsInited)
            {
                PivotItems[index].Refresh();
                PivotItems[index].IsInited = true;
            }
        }
    }
}

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
        private string header;
        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
                RaisePropertyChanged("Header");
            }
        }

        private ObservableCollection<HomeViewModelBase> pivotItems;
        public ObservableCollection<HomeViewModelBase> PivotItems
        {
            get
            {
                return pivotItems;
            }
            set
            {
                pivotItems = value;
                RaisePropertyChanged("PivotItems");
            }
        }

        public MainViewModel()
        {
            var baseViewModelList = new List<HomeViewModelBase>
            {
                new HomeViewModel(),
                new MentionsViewModel(),
            };
            PivotItems = new ObservableCollection<HomeViewModelBase>(baseViewModelList);
        }

        public void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            var pivot = sender as Pivot;
            int index = pivot.SelectedIndex;
            if (!PivotItems[index].IsLoaded)
            {
                PivotItems[index].Refresh();
                PivotItems[index].IsLoaded = true;
            }
        }
    }
}

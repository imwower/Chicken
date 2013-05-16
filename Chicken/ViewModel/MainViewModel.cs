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

namespace Chicken.ViewModel
{
    public class MainViewModel : NotificationObject
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

        private ObservableCollection<BaseViewModel> pivotList;
        public ObservableCollection<BaseViewModel> PivotList
        {
            get
            {
                return pivotList;
            }
            set
            {
                pivotList = value;
                RaisePropertyChanged("PivotList");
            }
        }

        public MainViewModel()
        {
            PivotList = new ObservableCollection<BaseViewModel>();
            PivotList.Add(new HomeViewModel());
        }
    }
}

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
using System.Windows.Navigation;
using System.Collections.ObjectModel;

namespace Chicken.ViewModel
{
    public class NavigationViewModelBase : NotificationObject
    {
        private string title = "Chicken";
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        private ObservableCollection<ViewModelBase> pivotItems;
        public ObservableCollection<ViewModelBase> PivotItems
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

        public NavigationContext NavigationContext { get; set; }

        public virtual void OnNavigatedTo(NavigationEventArgs e)
        {

        }
    }
}

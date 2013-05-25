using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Chicken.ViewModel
{
    public class PivotViewModelBase : NotificationObject
    {
        #region properties
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
        #endregion
    }
}

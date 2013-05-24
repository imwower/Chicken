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

        private int selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
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

        #region binding Command
        public ICommand ClickCommand
        {
            get
            {
                return new DelegateCommand(ClickDispatcher);
            }
        }
        #endregion

        #region dispatcher
        private void ClickDispatcher(object sender)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    Click(sender);
                });
        }
        #endregion

        #region virtual method
        public virtual void Click(object parameter)
        {
        }
        #endregion
    }
}

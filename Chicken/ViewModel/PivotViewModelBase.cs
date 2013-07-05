using System.Collections.ObjectModel;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Service;

namespace Chicken.ViewModel
{
    public class PivotViewModelBase : ViewModelBase
    {
        #region properties
        private string title;
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
            }
        }
        private AppBarState state;
        public AppBarState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                RaisePropertyChanged("State");
            }
        }
        private ObservableCollection<PivotItemViewModelBase> pivotItems;
        public ObservableCollection<PivotItemViewModelBase> PivotItems
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
        protected bool IsInit { get; set; }
        #endregion

        #region binding
        public ICommand SettingsCommand
        {
            get
            {
                return new DelegateCommand(SettingsAction);
            }
        }
        #endregion

        public PivotViewModelBase()
        {
            RefreshHandler = this.RefreshAction;
            ScrollToTopHandler = this.ScrollToTopAction;
            ScrollToBottomHandler = this.ScrollToBottomAction;
        }

        #region public method
        public virtual void MainPivot_LoadedPivotItem(int selectedIndex)
        {
            SelectedIndex = selectedIndex;
            if (!PivotItems[selectedIndex].IsInited)
            {
                Refresh();
            }
        }
        #endregion

        #region private method
        private void RefreshAction()
        {
            PivotItems[SelectedIndex].Refresh();
        }

        private void ScrollToTopAction()
        {
            PivotItems[SelectedIndex].ScrollToTop();
        }

        private void ScrollToBottomAction()
        {
            PivotItems[SelectedIndex].ScrollToBottom();
        }

        private void SettingsAction()
        {
            NavigationServiceManager.NavigateTo(PageNameEnum.SettingsPage);
        }
        #endregion
    }
}

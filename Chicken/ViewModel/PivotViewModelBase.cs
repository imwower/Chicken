using System.Collections.ObjectModel;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;

namespace Chicken.ViewModel
{
    public class PivotViewModelBase : NotificationObject
    {
        #region properties
        public int SelectedIndex { get; set; }
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
        public ObservableCollection<PivotItemViewModelBase> PivotItems { get; set; }
        protected bool IsInit { get; set; }
        #endregion

        #region binding
        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(RefreshAction);
            }
        }

        public ICommand TopCommand
        {
            get
            {
                return new DelegateCommand(ScrollToTopAction);
            }
        }

        public ICommand BottomCommand
        {
            get
            {
                return new DelegateCommand(ScrollToBottomAction);
            }
        }

        public ICommand NewTweetCommand
        {
            get
            {
                return new DelegateCommand(NewTweetAction);
            }
        }

        public ICommand NewMessageCommand
        {
            get
            {
                return new DelegateCommand(NewMessageAction);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new DelegateCommand(SearchAction);
            }
        }

        public ICommand MyProfileCommand
        {
            get
            {
                return new DelegateCommand(MyProfileAction);
            }
        }

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
        }

        #region public method
        public virtual void MainPivot_LoadedPivotItem()
        {
            if (!PivotItems[SelectedIndex].IsInited)
            {
                RefreshAction();
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

        private void NewTweetAction()
        {
            PivotItems[SelectedIndex].IsLoading = false;
            NavigationServiceManager.NavigateTo(Const.NewTweetPage);
        }

        private void NewMessageAction()
        {
            PivotItems[SelectedIndex].IsLoading = false;
            IsolatedStorageService.GetAndDeleteObject<NewMessageModel>(Const.NewMessagePage);
            NavigationServiceManager.NavigateTo(Const.NewMessagePage);
        }

        private void SearchAction()
        {
            PivotItems[SelectedIndex].IsLoading = false;
            NavigationServiceManager.NavigateTo(Const.SearchPage);
        }

        private void MyProfileAction()
        {
            PivotItems[SelectedIndex].IsLoading = false;
            IsolatedStorageService.GetAndDeleteObject<User>(Const.ProfilePage);
            NavigationServiceManager.NavigateTo(Const.ProfilePage);
        }

        private void SettingsAction()
        {
            PivotItems[SelectedIndex].IsLoading = false;
            NavigationServiceManager.NavigateTo(Const.SettingsPage);
        }
        #endregion
    }
}

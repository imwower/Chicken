using System.Windows;
using System.Windows.Input;

namespace Chicken.ViewModel
{
    public class ViewModelBase : NotificationObject
    {
        #region properties
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
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }
        private bool isInited;
        public bool IsInited
        {
            get
            {
                return isInited;
            }
            set
            {
                isInited = value;
                RaisePropertyChanged("IsInited");
            }
        }
        #endregion

        public ViewModelBase() { }

        #region binding Command
        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(RefreshDispatcher);
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                return new DelegateCommand(LoadDispatcher);
            }
        }

        public ICommand ClickCommand
        {
            get
            {
                return new DelegateCommand(ClickDispatcher);
            }
        }

        #endregion

        #region dispatcher
        private void RefreshDispatcher()
        {
            IsLoading = true;
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    Refresh();
                });
        }

        private void LoadDispatcher()
        {
            IsLoading = true;
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    Load();
                });
        }

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
        public virtual void Refresh()
        {
            IsLoading = true;
        }

        public virtual void Refreshed()
        {
            IsLoading = false;
            IsInited = true;
        }

        public virtual void Load()
        {
            IsLoading = true;
        }

        public virtual void Loaded()
        {
            IsLoading = false;
        }

        public virtual void Click(object parameter)
        {
        }
        #endregion
    }
}

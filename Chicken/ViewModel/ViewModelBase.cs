using System.Windows;
using System.Windows.Input;
using System;

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
                return new DelegateCommand(() => Dispatcher(Refresh));
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                return new DelegateCommand(() => Dispatcher(Load));
            }
        }

        public ICommand ClickCommand
        {
            get
            {
                return new DelegateCommand(o => Dispatcher(Click, o));
            }
        }

        public ICommand ItemClickCommand
        {
            get
            {
                return new DelegateCommand(o => Dispatcher<object>(ItemClick, o));
            }
        }

        #endregion

        #region dispatcher
        private void Dispatcher(Action action)
        {
            if (action == null)
            {
                return;
            }

            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    IsLoading = true;
                    action();
                });
        }

        private void Dispatcher<T>(Action<T> action, T parameter)
        {
            if (action == null)
            {
                return;
            }
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    IsLoading = true;
                    action(parameter);
                });
        }
        #endregion

        #region virtual method
        public virtual void Refresh()
        {
            IsLoading = false;
            IsInited = true;
        }
        
        public virtual void Load()
        {
            IsLoading = false;
        }

        public virtual void Click(object parameter)
        {
        }

        public virtual void ItemClick(object parameter)
        {
        }
        #endregion
    }
}

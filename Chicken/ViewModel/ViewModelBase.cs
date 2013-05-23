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
using System.Threading;
using Chicken.Service;

namespace Chicken.ViewModel
{
    public class ViewModelBase : NotificationObject
    {
        public Timer timer;

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
            timer = new Timer(
                (obj) =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () =>
                        {
                            Refresh();
                        });
                }, null, 1000, -1);
        }

        private void LoadDispatcher()
        {
            IsLoading = true;
            timer = new Timer(
                (obj) =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () =>
                        {
                            Load();
                        });
                }, null, 1000, -1);
        }

        private void ClickDispatcher(object sender)
        {
            IsLoading = true;
            timer = new Timer(
                (obj) =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () =>
                        {
                            Click(sender);
                        });
                }, null, 300, -1);
        }
        #endregion

        #region virtual method
        public virtual void Refresh()
        {

        }

        public virtual void Refreshed()
        {
            IsLoading = false;
            IsInited = true;
        }

        public virtual void Load()
        {
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

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
        private bool isLoaded;
        public bool IsLoaded
        {
            get
            {
                return isLoaded;
            }
            set
            {
                isLoaded = value;
                RaisePropertyChanged("IsLoaded");
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
                            IsLoading = false;
                        });
                }, null, 1000, -1);
        }
        #endregion

        #region virtual method
        public virtual void Refresh()
        {
            IsLoaded = true;
        }
        #endregion
    }
}

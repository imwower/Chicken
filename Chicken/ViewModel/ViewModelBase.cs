using System.Windows;
using System.Windows.Input;
using System;
using System.ComponentModel;
using System.Threading;

namespace Chicken.ViewModel
{
    public class ViewModelBase : NotificationObject
    {
        #region event handler
        protected EventHandler RefreshHandler;
        protected EventHandler LoadHandler;
        protected EventHandler ClickHandler;
        #endregion

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

        #region private
        BackgroundWorker worker = new BackgroundWorker();
        #endregion

        public ViewModelBase()
        {
            worker.WorkerSupportsCancellation = true;
        }

        #region binding Command
        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(Refresh);
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                return new DelegateCommand(Load);
            }
        }

        public ICommand ClickCommand
        {
            get
            {
                return new DelegateCommand(Click);
            }
        }

        public ICommand ItemClickCommand
        {
            get
            {
                return new DelegateCommand(Worker_ItemClick);
            }
        }

        #endregion

        #region public method
        public virtual void Refresh()
        {
            if (RefreshHandler == null)
            {
                return;
            }
            if (worker.IsBusy)
            {
                Thread.Sleep(1000);
                return;
            }
            else
            {
                IsLoading = true;
                worker.DoWork += DoRefresh;
                worker.RunWorkerCompleted += RefreshCompleted;
                worker.RunWorkerAsync();
            }
        }

        public virtual void Load()
        {
            if (LoadHandler == null)
            {
                return;
            }
            if (worker.IsBusy)
            {
                Thread.Sleep(1000);
                return;
            }
            else
            {
                worker.DoWork += DoLoad;
                worker.RunWorkerCompleted += LoadCompleted;
                worker.RunWorkerAsync();
                IsLoading = true;
            }
        }

        private void Click(object parameter)
        {
            if (ClickHandler == null)
            {
                return;
            }
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
            IsLoading = false;
            ClickHandler(parameter, null);
        }
        #endregion


        #region private method
        #region refresh
        private void DoRefresh(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2000);
            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    RefreshHandler(sender, e);
                    Refreshed();
                });
        }

        private void RefreshCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.DoWork -= DoRefresh;
        }
        #endregion

        #region load
        private void DoLoad(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2000);
            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    LoadHandler(sender, e);
                    Loaded();
                });
        }

        private void LoadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.DoWork -= DoLoad;
        }
        #endregion


        #endregion






        #region click

        #endregion

        #region item click
        private void Worker_ItemClick(object parameter)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
            Thread.Sleep(1000);
            IsLoading = false;
            Dispatcher(ItemClick, parameter);
        }
        #endregion

        #region dispatcher
        private static void Dispatcher(Action action)
        {
            if (action == null)
            {
                return;
            }
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    action();
                });
        }

        private static void Dispatcher<T>(Action<T> action, T parameter)
        {
            if (action == null)
            {
                return;
            }
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    action(parameter);
                });
        }
        #endregion


        #region virtual method
        //public virtual void Refresh()
        //{
        //}

        public virtual void Refreshed()
        {
            IsLoading = false;
            IsInited = true;
        }

        //public virtual void Load()
        //{
        //}

        public virtual void Loaded()
        {
            IsLoading = false;
        }

        //public virtual void Click(object parameter)
        //{
        //}

        public virtual void ItemClick(object parameter)
        {
        }
        #endregion
    }
}

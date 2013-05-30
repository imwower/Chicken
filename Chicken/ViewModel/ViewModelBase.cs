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
        protected delegate void LoadEventHandler();
        protected delegate void ClickEventHandler(object parameter);
        protected LoadEventHandler RefreshHandler;
        protected LoadEventHandler LoadHandler;
        protected ClickEventHandler ClickHandler;
        protected ClickEventHandler ItemClickHandler;
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
                return new DelegateCommand(ItemClick);
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
            Thread.Sleep(1000);
            if (worker.IsBusy)
            {
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
            Thread.Sleep(1000);
            if (worker.IsBusy)
            {
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

        public virtual void Click(object parameter)
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
            ClickHandler(parameter);
        }

        public virtual void ItemClick(object parameter)
        {
            if (ItemClickHandler == null)
            {
                return;
            }
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
            IsLoading = false;
            ItemClickHandler(parameter);
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
                    RefreshHandler();
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
                    LoadHandler();
                    Loaded();
                });
        }

        private void LoadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.DoWork -= DoLoad;
        }
        #endregion
        #endregion

        #region
        private void Refreshed()
        {
            IsLoading = false;
            IsInited = true;
        }

        private void Loaded()
        {
            IsLoading = false;
        }
        #endregion
    }
}

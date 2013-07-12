using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Chicken.Common;

namespace Chicken.ViewModel
{
    public class PivotItemViewModelBase : NotificationObject
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
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    RaisePropertyChanged("IsLoading");
                }
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
        private ScrollTo scrollTo;
        public ScrollTo ScrollTo
        {
            get
            {
                return scrollTo;
            }
            set
            {
                scrollTo = value;
                RaisePropertyChanged("ScrollTo");
            }
        }
        #endregion

        #region private
        BackgroundWorker worker = new BackgroundWorker();
        #endregion

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

        public ICommand TopCommand
        {
            get
            {
                return new DelegateCommand(ScrollToTop);
            }
        }

        public ICommand BottomCommand
        {
            get
            {
                return new DelegateCommand(ScrollToBottom);
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

        public PivotItemViewModelBase()
        {
            worker.WorkerSupportsCancellation = true;
        }

        #region public method
        public void Load()
        {
            if (LoadHandler == null
                || worker.IsBusy)
                return;
            if (!isLoading)
                IsLoading = true;
            worker.DoWork += DoLoad;
            worker.RunWorkerCompleted += LoadCompleted;
            worker.RunWorkerAsync();
        }

        public void Refresh()
        {
            if (RefreshHandler == null
                || worker.IsBusy)
                return;
            if (!isLoading)
                IsLoading = true;
            worker.DoWork += DoRefresh;
            worker.RunWorkerCompleted += RefreshCompleted;
            worker.RunWorkerAsync();
        }

        public void ScrollToTop()
        {
            //if (ScrollTo < 0)
            //    ScrollTo -= 1;
            //else
            ScrollTo = ScrollTo.Top;
        }

        public void ScrollToBottom()
        {
            //if (ScrollTo > 0)
            //    ScrollTo += 1;
            //else
            ScrollTo = ScrollTo.Bottom;
        }

        public void Click(object parameter)
        {
            if (ClickHandler == null)
                return;
            if (worker.IsBusy)
                worker.CancelAsync();
            IsLoading = false;
            ClickHandler(parameter);
        }

        public void ItemClick(object parameter)
        {
            if (ItemClickHandler == null)
                return;
            if (worker.IsBusy)
                worker.CancelAsync();
            IsLoading = false;
            ItemClickHandler(parameter);
        }
        #endregion

        #region private method
        #region refresh
        private void DoRefresh(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            Deployment.Current.Dispatcher.BeginInvoke(RefreshHandler);
        }

        private void RefreshCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.DoWork -= DoRefresh;
            worker.RunWorkerCompleted -= RefreshCompleted;
        }
        #endregion

        #region load
        private void DoLoad(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            Deployment.Current.Dispatcher.BeginInvoke(LoadHandler);
        }

        private void LoadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.DoWork -= DoLoad;
            worker.RunWorkerCompleted -= LoadCompleted;
        }
        #endregion
        #endregion

        #region refreshed
        protected void Refreshed()
        {
            IsLoading = false;
            IsInited = true;
        }

        protected void Loaded()
        {
            IsLoading = false;
        }
        #endregion
    }
}

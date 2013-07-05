﻿using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;

namespace Chicken.ViewModel
{
    public class PivotItemViewModelBase : NotificationObject
    {
        #region event handler
        protected delegate void LoadEventHandler();
        protected delegate void ClickEventHandler(object parameter);
        public delegate void ToastMessageEventHandler(ToastMessage toastMessage);
        protected LoadEventHandler RefreshHandler;
        protected LoadEventHandler LoadHandler;
        protected ClickEventHandler ClickHandler;
        protected ClickEventHandler ItemClickHandler;
        public ToastMessageEventHandler ToastMessageHandler;
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
                switch (value)
                {
                    case ScrollTo.Top:
                        if (scrollTo < 0)
                            scrollTo -= 1;
                        else
                            scrollTo = value;
                        break;
                    case ScrollTo.Bottom:
                        if (scrollTo > 0)
                            scrollTo += 1;
                        else
                            scrollTo = value;
                        break;
                }
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
                IsLoading = true;
                worker.DoWork += DoLoad;
                worker.RunWorkerCompleted += LoadCompleted;
                worker.RunWorkerAsync();
            }
        }

        public void Refresh()
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

        public void ScrollToTop()
        {
            ScrollTo = ScrollTo.Top;
        }

        public void ScrollToBottom()
        {
            ScrollTo = ScrollTo.Bottom;
        }

        public void Click(object parameter)
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

        public void ItemClick(object parameter)
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

        public void HandleMessage(ToastMessage message)
        {
            if (ToastMessageHandler != null)
            {
                ToastMessageHandler(message);
            }
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
                });
        }

        private void LoadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.DoWork -= DoLoad;
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

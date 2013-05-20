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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Controls;
using System.Threading;

namespace Chicken.ViewModel
{
    public abstract class BaseViewModel : NotificationObject
    {
        #region private properties
        Timer timer;
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

        private ObservableCollection<TweetViewModel> tweetList = new ObservableCollection<TweetViewModel>();
        public ObservableCollection<TweetViewModel> TweetList
        {
            get
            {
                return tweetList;
            }
            set
            {
                if (value != tweetList)
                {
                    tweetList = value;
                    RaisePropertyChanged("TweetList");
                }
            }
        }

        #endregion

        public BaseViewModel()
        {
            TweetList = new ObservableCollection<TweetViewModel>();
        }

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
                            IsLoading = false;
                        });
                }, null, 1000, -1);
        }
        #endregion

        #region virtual method
        public virtual void Refresh()
        {
        }

        public virtual void Load()
        {
        }
        #endregion
    }
}

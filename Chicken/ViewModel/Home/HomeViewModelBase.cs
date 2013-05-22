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
using Microsoft.Phone.Controls;
using Chicken.Service;

namespace Chicken.ViewModel.Home
{
    public class HomeViewModelBase : ViewModelBase
    {
        #region properties
        private ObservableCollection<TweetViewModel> tweetList;
        public ObservableCollection<TweetViewModel> TweetList
        {
            get
            {
                return tweetList;
            }
            set
            {
                tweetList = value;
                RaisePropertyChanged("TweetList");
            }
        }
        #endregion

        public HomeViewModelBase() { }

        #region binding Command
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

        /// <summary>
        /// sender is User's Id
        /// </summary>
        /// <param name="sender">int</param>
        private void ClickDispatcher(object sender)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    NavigationService.NavigateTo(NavigationService.ProfilePage, "?id=" + sender);
                });
        }
        #endregion

        #region virtual method
        public virtual void Load()
        {
        }
        #endregion
    }
}

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
        #endregion

        #region dispatcher
        #endregion

        #region virtual method
        #endregion
    }
}

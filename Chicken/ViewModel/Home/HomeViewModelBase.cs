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
using Chicken.Common;

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
        /// <summary>
        /// navigate to profile page
        /// </summary>
        /// <param name="parameter">user id</param>
        public override void Click(object parameter)
        {
            IsLoading = false;
            Dictionary<string, object> parameters = new Dictionary<string, object>(1);
            parameters.Add(TwitterHelper.USER_ID, parameter);
            string uri = TwitterHelper.GenerateRelativeUri(TwitterHelper.ProfilePage, parameters);
            TwitterHelper.NavigateTo(uri);
        }
        #endregion
    }
}

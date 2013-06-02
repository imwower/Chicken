using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Home.Base;
using System;

namespace Chicken.ViewModel.Home
{
    public class HomeViewModelBase : PivotItemViewModelBase
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

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public HomeViewModelBase()
        {
            ClickHandler = ClickAction;
        }

        /// <summary>
        /// navigate to profile detail page
        /// </summary>
        /// <param name="parameter">user id</param>
        private void ClickAction(object parameter)
        {
            IsLoading = false;
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.USER_ID, parameter);
            NavigationServiceManager.NavigateTo(Const.ProfilePage, parameters);
        }

        public override void ItemClick(object parameter)
        {
            IsLoading = false;
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.ID, parameter);
            NavigationServiceManager.NavigateTo(Const.StatusPage, parameters);
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Home.Base;

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

        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        public HomeViewModelBase() { }

        /// <summary>
        /// navigate to profile detail page
        /// </summary>
        /// <param name="parameter">user id</param>
        public override void Click(object parameter)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.USER_ID, parameter);
            NavigationServiceManager.NavigateTo(Const.ProfilePage, parameters);
        }

        public override void ItemClick(object parameter)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.ID, parameter);
            NavigationServiceManager.NavigateTo(Const.StatusPage, parameters);
        }
    }
}

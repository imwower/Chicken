using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Home.Base;

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
            ItemClickHandler = this.ItemClickAction;
        }

        private void ClickAction(object parameter)
        {
            NavigationServiceManager.NavigateTo(PageNameEnum.ProfilePage, parameter);
        }

        private void ItemClickAction(object parameter)
        {
            NavigationServiceManager.NavigateTo(PageNameEnum.StatusPage, parameter);
        }
    }
}

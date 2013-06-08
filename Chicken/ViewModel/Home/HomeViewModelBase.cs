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
            var userViewModel = parameter as User;
            var user = new UserModel
            {
                Id = userViewModel.Id,
                Name = userViewModel.Name,
                ScreenName = userViewModel.ScreenName,
            };
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.ProfilePage, user);
        }

        private void ItemClickAction(object parameter)
        {
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.StatusPage, parameter);
        }
    }
}

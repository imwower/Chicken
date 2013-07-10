using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Home
{
    public class HomeViewModelBase : PivotItemViewModelBase
    {
        #region properties
        public ObservableCollection<TweetViewModel> TweetList { get; set; }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public HomeViewModelBase()
        {
            ClickHandler = ClickAction;
            ItemClickHandler = this.ItemClickAction;
        }

        #region public methods
        public virtual void NewTweet()
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(Const.NewTweetPage);
        }

        public virtual void NewMessage()
        {
            IsLoading = false;
            IsolatedStorageService.GetAndDeleteObject<NewMessageModel>(Const.NewMessagePage);
            NavigationServiceManager.NavigateTo(Const.NewMessagePage);
        }

        public virtual void MyProfile()
        {
            IsLoading = false;
            IsolatedStorageService.GetAndDeleteObject<User>(Const.ProfilePage);
            NavigationServiceManager.NavigateTo(Const.ProfilePage);
        }
        #endregion

        #region actions
        private void ClickAction(object parameter)
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(Const.ProfilePage, parameter);
        }

        private void ItemClickAction(object parameter)
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(Const.StatusPage, parameter);
        }
        #endregion
    }
}

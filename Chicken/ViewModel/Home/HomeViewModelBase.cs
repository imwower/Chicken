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
            NavigationServiceManager.NavigateTo(PageNameEnum.NewTweetPage);
        }

        public virtual void NewMessage()
        {
            IsLoading = false;
            IsolatedStorageService.GetAndDeleteObject<NewMessageModel>(PageNameEnum.NewMessagePage);
            NavigationServiceManager.NavigateTo(PageNameEnum.NewMessagePage);
        }

        public virtual void MyProfile()
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(PageNameEnum.ProfilePage, App.AuthenticatedUser);
        }
        #endregion

        #region actions
        private void ClickAction(object parameter)
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(PageNameEnum.ProfilePage, parameter);
        }

        private void ItemClickAction(object parameter)
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(PageNameEnum.StatusPage, parameter);
        }
        #endregion
    }
}

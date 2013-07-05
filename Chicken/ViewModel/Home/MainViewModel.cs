using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Home.VM;

namespace Chicken.ViewModel.Home
{
    public class MainViewModel : PivotViewModelBase
    {
        #region binding
        public ICommand NewTweetCommand
        {
            get
            {
                return new DelegateCommand(NewTweetAction);
            }
        }

        public ICommand NewMessageCommand
        {
            get
            {
                return new DelegateCommand(NewMessageAction);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new DelegateCommand(SearchAction);
            }
        }

        public ICommand MyProfileCommand
        {
            get
            {
                return new DelegateCommand(MyProfileAction);
            }
        }
        #endregion

        public MainViewModel()
        {
            var baseViewModelList = new List<PivotItemViewModelBase>
            {
                new HomeViewModel(),
                new MentionsViewModel(),
                new DMsPivotViewModel(),
            };
            PivotItems = new ObservableCollection<PivotItemViewModelBase>(baseViewModelList);
        }

        #region actions
        private void NewTweetAction()
        {
            NavigationServiceManager.NavigateTo(PageNameEnum.NewTweetPage);
        }

        private void NewMessageAction()
        {
            IsolatedStorageService.GetAndDeleteObject<NewMessageModel>(PageNameEnum.NewMessagePage);
            NavigationServiceManager.NavigateTo(PageNameEnum.NewMessagePage);
        }

        private void SearchAction()
        { }

        private void MyProfileAction()
        {
            NavigationServiceManager.NavigateTo(PageNameEnum.ProfilePage, App.AuthenticatedUser);
        }
        #endregion
    }
}

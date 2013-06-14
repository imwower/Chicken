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
                return new DelegateCommand(NewTweet);
            }
        }

        public ICommand NewMessageCommand
        {
            get
            {
                return new DelegateCommand(NewMessage);
            }
        }
        #endregion

        public MainViewModel()
        {
            Title = "Chicken";
            var baseViewModelList = new List<PivotItemViewModelBase>
            {
                new HomeViewModel(),
                new MentionsViewModel(),
                new DMsPivotViewModel(),
            };
            PivotItems = new ObservableCollection<PivotItemViewModelBase>(baseViewModelList);
        }

        private void NewTweet()
        {
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.NewTweetPage);
        }

        private void NewMessage()
        {
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.NewMessagePage);
        }
    }
}

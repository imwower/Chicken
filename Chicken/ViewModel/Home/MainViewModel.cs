using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
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
            (PivotItems[SelectedIndex] as HomeViewModelBase).NewTweet();
        }

        private void NewMessageAction()
        {
            (PivotItems[SelectedIndex] as HomeViewModelBase).NewMessage();
        }

        private void SearchAction()
        {
            //TODO: search
        }

        private void MyProfileAction()
        {
            (PivotItems[SelectedIndex] as HomeViewModelBase).MyProfile();
        }
        #endregion
    }
}

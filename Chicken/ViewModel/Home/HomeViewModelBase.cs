using System.Collections.ObjectModel;
using Chicken.Service;
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
    }
}

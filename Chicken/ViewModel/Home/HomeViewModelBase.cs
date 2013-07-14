using System.Collections.ObjectModel;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Home
{
    public class HomeViewModelBase : PivotItemViewModelBase
    {
        #region properties
        public ObservableCollection<TweetViewModel> TweetList { get; set; }
        #endregion

        public HomeViewModelBase()
        {
        }
    }
}

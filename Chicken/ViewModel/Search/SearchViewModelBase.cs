using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Service;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Search
{
    public class SearchViewModelBase : PivotItemViewModelBase
    {
        #region properties
        protected string SearchQuery { get; set; }
        public ObservableCollection<TweetViewModel> TweetList { get; set; }
        public ObservableCollection<UserProfileViewModel> UserList { get; set; }
        #endregion

        #region public method
        public virtual void Search()
        {
        }
        #endregion

        #region protect method
        protected bool CheckAndGetSearchQuery()
        {
            var searchQuery = IsolatedStorageService.GetObject<string>(Const.SearchPage);
            if (string.IsNullOrEmpty(searchQuery) || searchQuery == SearchQuery)
            {
                base.Refreshed();
                return false;
            }
            if (TweetList != null)
                TweetList.Clear();
            return true;
        }
        #endregion
    }
}

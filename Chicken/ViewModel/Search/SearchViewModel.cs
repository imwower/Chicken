using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.ViewModel.Search.VM;
using System.Windows.Input;
using Chicken.Service;
using Chicken.Common;

namespace Chicken.ViewModel.Search
{
    public class SearchViewModel : PivotViewModelBase
    {
        #region properties
        public string SearchQuery { get; set; }
        #endregion

        #region binding
        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(SearchAction);
            }
        }
        #endregion

        public SearchViewModel()
        {
            var baseViewModelList = new List<PivotItemViewModelBase>
            {
                new SearchTweetViewModel(),
                new SearchUserViewModel()
            };
            PivotItems = new ObservableCollection<PivotItemViewModelBase>(baseViewModelList);
        }

        #region actions
        private void SearchAction()
        {
            if (string.IsNullOrEmpty(SearchQuery))
                return;
            IsolatedStorageService.CreateObject(Const.SearchPage, SearchQuery);
            (PivotItems[SelectedIndex] as SearchViewModelBase).Search();
        }
        #endregion
    }
}

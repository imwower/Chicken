﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Service;
using Chicken.ViewModel.Search.VM;

namespace Chicken.ViewModel.Search
{
    public class SearchViewModel : PivotViewModelBase
    {
        #region properties
        private string searchQuery;
        public string SearchQuery
        {
            get
            {
                return searchQuery;
            }
            set
            {
                searchQuery = value;
                RaisePropertyChanged("SearchQuery");
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

        #region public method
        public void Search()
        {
            if (string.IsNullOrEmpty(SearchQuery))
                return;
            IsolatedStorageService.CreateObject(Const.SearchPage, SearchQuery);
            (PivotItems[SelectedIndex] as SearchViewModelBase).Search();
        }
        #endregion

        #region override
        public override void MainPivot_LoadedPivotItem()
        {
            if (!IsInit)
            {
                SearchQuery = IsolatedStorageService.GetObject<string>(Const.SearchPage);
                IsInit = true;
            }
            base.MainPivot_LoadedPivotItem();
        }
        #endregion
    }
}

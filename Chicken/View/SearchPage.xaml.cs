using System;
using Chicken.ViewModel.Search;

namespace Chicken.View
{
    public partial class SearchPage : PivotPageBase
    {
        #region properties
        private SearchViewModel searchViewModel;
        #endregion

        public SearchPage()
        {
            InitializeComponent();
            Pivot = this.MainPivot;
            searchViewModel = new SearchViewModel();
            PivotViewModelBase = searchViewModel;
            base.Init();
        }

        #region search textbox
        private void SearchBox_EnterKeyDown(object sender, EventArgs e)
        {
            this.Focus();
            searchViewModel.Search();
        }
        #endregion
    }
}
using System.Windows.Input;
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
        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
                searchViewModel.Search();
            }
        }
        #endregion
    }
}
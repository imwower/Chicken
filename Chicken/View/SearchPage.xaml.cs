using Chicken.ViewModel.Search;

namespace Chicken.View
{
    public partial class SearchPage : PivotPageBase
    {
        public SearchPage()
        {
            InitializeComponent();
            Pivot = this.MainPivot;
            PivotViewModelBase = new SearchViewModel();
            base.Init();
        }
    }
}
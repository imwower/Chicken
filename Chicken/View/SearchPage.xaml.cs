using System.Windows.Controls;
using System.Windows.Input;
using Chicken.ViewModel.Search;

namespace Chicken.View
{
    public partial class SearchPage : PivotPageBase
    {
        private SearchViewModel searchViewModel;

        public SearchPage()
        {
            InitializeComponent();
            Pivot = this.MainPivot;
            searchViewModel = new SearchViewModel();
            PivotViewModelBase = searchViewModel;
            base.Init();
        }

        #region search textbox
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            var binding = textbox.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

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
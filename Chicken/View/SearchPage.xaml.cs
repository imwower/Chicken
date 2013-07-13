using System.Windows.Controls;
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

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            var binding = textbox.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
    }
}
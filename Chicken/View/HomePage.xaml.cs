using Chicken.ViewModel.Home;

namespace Chicken.View
{
    public partial class HomePage : PivotPageBase
    {
        public HomePage()
        {
            InitializeComponent();
            Pivot = this.MainPivot;
            PivotViewModelBase = new MainViewModel();
            base.Init();
            this.BackKeyPress += Page_OnBackKeyPress;
        }
    }
}
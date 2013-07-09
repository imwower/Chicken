using Chicken.ViewModel.Status;

namespace Chicken.View
{
    public partial class StatusPage : PivotPageBase
    {
        public StatusPage()
        {
            InitializeComponent();
            Pivot = this.MainPivot;
            PivotViewModelBase = new StatusViewModel();
            base.Init();
        }
    }
}
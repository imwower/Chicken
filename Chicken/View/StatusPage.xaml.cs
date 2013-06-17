using System.Windows.Navigation;
using Chicken.Common;
using Chicken.Service;
using Chicken.ViewModel;
using Chicken.ViewModel.Status;
using Microsoft.Phone.Controls;

namespace Chicken.View
{
    public partial class StatusPage : PivotPageBase
    {
        protected override Pivot Pivot
        {
            get
            {
                return this.MainPivot;
            }
        }
        private StatusViewModel statusViewModel;
        protected override PivotViewModelBase PivotViewModelBase
        {
            get
            {
                return this.statusViewModel;
            }
        }

        public StatusPage()
        {
            InitializeComponent();
            statusViewModel = new StatusViewModel();
            this.MainPivot.DataContext = statusViewModel;
            base.Init();
        }
    }
}
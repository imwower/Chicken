using Chicken.ViewModel;
using Chicken.ViewModel.Home;
using Microsoft.Phone.Controls;

namespace Chicken.View
{
    public partial class MainPage : PivotPageBase
    {
        protected override Pivot Pivot
        {
            get
            {
                return this.MainPivot;
            }
        }
        private MainViewModel mainViewModel;
        protected override PivotViewModelBase PivotViewModelBase
        {
            get
            {
                return this.mainViewModel;
            }
        }

        public MainPage()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel();
            this.MainPivot.DataContext = mainViewModel;
            base.Init();
        }
    }
}
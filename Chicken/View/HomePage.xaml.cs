using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Chicken.ViewModel;
using Chicken.ViewModel.Home;
using Microsoft.Phone.Controls;

namespace Chicken.View
{
    public partial class HomePage : PivotPageBase
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

        public HomePage()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel();
            this.MainPivot.DataContext = mainViewModel;
            base.Init();
            this.BackKeyPress += HomePage_BackKeyPress;
        }

        void HomePage_BackKeyPress(object sender, CancelEventArgs e)
        {
            int count = NavigationService.BackStack.Count();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

            if (MessageBox.Show("Are you sure to exit?", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
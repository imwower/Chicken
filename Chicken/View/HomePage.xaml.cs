using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
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
            this.BackKeyPress += HomePage_BackKeyPress;
        }

        private void HomePage_BackKeyPress(object sender, CancelEventArgs e)
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
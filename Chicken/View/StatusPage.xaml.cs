using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Chicken.Service.Interface;
using Chicken.ViewModel.Status;
using Microsoft.Phone.Controls;
using Chicken.Common;

namespace Chicken.View
{
    public partial class StatusPage : PhoneApplicationPage, INavigationService
    {
        StatusViewModel statusViewModel;

        public StatusPage()
        {
            InitializeComponent();
            statusViewModel = new StatusViewModel();
            this.MainPivot.DataContext = statusViewModel;
            this.MainPivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(MainPivot_LoadedPivotItem);
        }

        void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            int selectedIndex = (sender as Pivot).SelectedIndex;
            Dispatcher.BeginInvoke(() =>
            {
                statusViewModel.MainPivot_LoadedPivotItem(selectedIndex);
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string statusId = NavigationContext.QueryString[Const.ID];
            statusViewModel.OnNavigatedTo(statusId);
        }

        public void ChangeSelectedIndex(int selectedIndex, IDictionary<string, object> parameters = null)
        {
            this.MainPivot.SelectedIndex = selectedIndex;
        }
    }
}
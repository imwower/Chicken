using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Chicken.Service;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Chicken.Common;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModel : NavigationViewModelBase
    {
        private int selectedIndex = 1;
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }

        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        #region properties
        string userId;
        #endregion

        public ProfileViewModel()
        {
            var baseViewModelList = new List<ViewModelBase>
            {
                new ProfilePivotViewModel(),
                new UserTweetsViewModel(),
            };
            this.PivotItems = new ObservableCollection<ViewModelBase>(baseViewModelList);
        }

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            userId = NavigationContext.QueryString[TwitterHelper.USER_ID];
        }

        public void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            var pivot = sender as Pivot;
            int index = pivot.SelectedIndex;
            if (!PivotItems[index].IsInited)
            {
                PivotItems[index].IsLoading = true;
                (PivotItems[index] as ProfileViewModelBase).UserId = userId;
                PivotItems[index].Refresh();
                PivotItems[index].IsInited = true;
            }
        }
    }
}

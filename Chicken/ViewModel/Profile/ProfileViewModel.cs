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
    public class ProfileViewModel : PivotViewModelBase
    {
        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        #region properties
        private string userId;
        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                RaisePropertyChanged("UserId");
            }
        }
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

        public override void Click(object parameter)
        {
            if (UserId==parameter.ToString())
            {
                SelectedIndex = 0;
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>(1);
                parameters.Add(TwitterHelper.USER_ID, parameter);
                string uri = TwitterHelper.GenerateRelativeUri(TwitterHelper.ProfilePage, parameters);
                TwitterHelper.NavigateTo(uri);
            }
        }

        public void OnNavigatedTo(string userId)
        {
            UserId = userId;
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

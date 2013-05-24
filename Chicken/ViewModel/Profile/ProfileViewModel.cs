using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Service;
using Chicken.ViewModel.Profile.VM;

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
                new UserFollowingViewModel(),
            };
            this.PivotItems = new ObservableCollection<ViewModelBase>(baseViewModelList);
        }

        public override void Click(object parameter)
        {
            if (UserId == parameter.ToString())
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

        public void MainPivot_LoadedPivotItem()
        {
            if (!PivotItems[SelectedIndex].IsInited)
            {
                (PivotItems[SelectedIndex] as ProfileViewModelBase).UserId = userId;
                PivotItems[SelectedIndex].Refresh();
            }
        }
    }
}

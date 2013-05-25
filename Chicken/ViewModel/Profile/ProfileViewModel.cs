using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Service;
using Chicken.Service.Interface;
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
                new ProfileDetailViewModel(),
                new UserTweetsViewModel(),
                new UserFollowingViewModel(),
                new UserFollowersViewModel(),
                new UserFavouritesViewModel(),
            };
            this.PivotItems = new ObservableCollection<ViewModelBase>(baseViewModelList);
        }

        public void OnNavigatedTo(string userId)
        {
            UserId = userId;
        }

        public void MainPivot_LoadedPivotItem(int selectedIndex)
        {
            if (!PivotItems[selectedIndex].IsInited)
            {
                (PivotItems[selectedIndex] as ProfileViewModelBase).UserId = userId;
                PivotItems[selectedIndex].Refresh();
            }
        }
    }
}

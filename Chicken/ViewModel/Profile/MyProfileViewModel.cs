using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Profile.VM;

namespace Chicken.ViewModel.Profile
{
    public class MyProfileViewModel : PivotViewModelBase
    {
        #region properties
        private User user;
        public User User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                RaisePropertyChanged("User");
            }
        }
        #endregion

        #region binding
        public ICommand EditMyProfileCommand
        {
            get
            {
                return new DelegateCommand(EditMyProfileAction);
            }
        }

        public ICommand BlocksCommand
        {
            get
            {
                return new DelegateCommand(BlocksAction);
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public MyProfileViewModel()
        {
            var baseViewModelList = new List<PivotItemViewModelBase>
            {
                new ProfileDetailViewModel(){ IsMyself = true },
                new UserTweetsViewModel(),
                new UserFollowingViewModel(),
                new UserFollowersViewModel(),
                new UserFavoritesViewModel(),
            };
            this.PivotItems = new ObservableCollection<PivotItemViewModelBase>(baseViewModelList);
        }

        public override void MainPivot_LoadedPivotItem(int selectedIndex)
        {
            if (User == null)
                User = IsolatedStorageService.GetAuthenticatedUser();
            if (User != null)
            {
                (PivotItems[selectedIndex] as ProfileViewModelBase).UserProfile = User;
                base.MainPivot_LoadedPivotItem(selectedIndex);
            }
            else
            {
                TweetService.GetMyProfileDetail<User>(
                    profile =>
                    {
                        User = profile;
                        (PivotItems[selectedIndex] as ProfileViewModelBase).UserProfile = User;
                        base.MainPivot_LoadedPivotItem(selectedIndex);
                        IsolatedStorageService.CreateAuthenticatedUser(User);
                    });
            }
        }

        #region actions
        private void EditMyProfileAction()
        { }

        private void BlocksAction()
        { }
        #endregion
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Profile.VM;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModel : PivotViewModelBase
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
        private string followButtonText;
        public string FollowButtonText
        {
            get
            {
                return followButtonText;
            }
            set
            {
                followButtonText = value;
                RaisePropertyChanged("FollowButtonText");
            }
        }
        private AppBarState state = AppBarState.ProfileDefault;
        public AppBarState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                RaisePropertyChanged("State");
            }
        }
        #endregion

        #region binding
        public ICommand MentionCommand
        {
            get
            {
                return new DelegateCommand(MentionAction);
            }
        }

        public ICommand MessageCommand
        {
            get
            {
                return new DelegateCommand(NewMessageAction);
            }
        }

        public ICommand FollowCommand
        {
            get
            {
                return new DelegateCommand(FollowAction);
            }
        }

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

        public ProfileViewModel()
        {
            var baseViewModelList = new List<PivotItemViewModelBase>
            {
                new ProfileDetailViewModel(),
                new UserTweetsViewModel(),
                new UserFollowingViewModel(),
                new UserFollowersViewModel(),
                new UserFavoritesViewModel(),
            };
            this.PivotItems = new ObservableCollection<PivotItemViewModelBase>(baseViewModelList);
        }

        public override void MainPivot_LoadedPivotItem(int selectedIndex)
        {
            #region init from file
            if (User == null)
            {
                User = IsolatedStorageService.GetObject<User>(PageNameEnum.ProfilePage);
            }
            #endregion
            #region is myself or not; change appbar menu
            if (User.Id == App.AuthenticatedUser.Id)
            {
                User.IsMyself = true;
                if (selectedIndex == 0)
                {
                    State = AppBarState.MyProfileDefault;
                }
                else
                {
                    State = AppBarState.MyProfileWithEdit;
                }
            }
            else
            {
                if (User.IsFollowing)
                {
                    FollowButtonText = "unfollow";
                }
                else
                {
                    FollowButtonText = "follow";
                }
                if (selectedIndex == 0)
                {
                    State = AppBarState.ProfileDefault;
                }
                else
                {
                    State = AppBarState.ProfileWithRefresh;
                }
            }
            #endregion
            (PivotItems[selectedIndex] as ProfileViewModelBase).UserProfile = User;
            base.MainPivot_LoadedPivotItem(selectedIndex);
        }

        private void MentionAction()
        {
            (PivotItems[SelectedIndex] as ProfileViewModelBase).Mention();
        }

        private void NewMessageAction()
        {
            (PivotItems[SelectedIndex] as ProfileViewModelBase).NewMessage();
        }

        private void FollowAction()
        {
            (PivotItems[SelectedIndex] as ProfileViewModelBase).Follow();
        }

        private void EditMyProfileAction()
        {
            (PivotItems[SelectedIndex] as ProfileViewModelBase).EditMyProfile();
        }

        private void BlocksAction()
        {
            //TODO
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Base;
using Chicken.ViewModel.Profile.VM;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModel : PivotViewModelBase
    {
        #region properties
        public UserProfileDetailViewModel User { get; set; }
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
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
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
            if (IsInit)
            {
                SwitchAppBar(selectedIndex);
                return;
            }
            PivotItems[selectedIndex].IsLoading = true;
            var selectedUser = IsolatedStorageService.GetObject<User>(PageNameEnum.ProfilePage);
            TweetService.GetUserProfileDetail(selectedUser,
                profile =>
                {
                    if (profile.HasError)
                    {
                        PivotItems[selectedIndex].IsLoading = false;
                        return;
                    }
                    this.User = new UserProfileDetailViewModel(profile);
                    if (User.Id == App.AuthenticatedUser.Id)
                        User.IsMyself = true;
                    SwitchAppBar(selectedIndex);
                    ChangeFollowButtonText();
                    IsInit = true;
                });
        }

        #region actions
        private void MentionAction()
        {
            (PivotItems[SelectedIndex] as ProfileViewModelBase).Mention();
        }

        private void NewMessageAction()
        {
            (PivotItems[SelectedIndex] as ProfileViewModelBase).NewMessage();
        }

        /// <summary>
        /// follow or unfollow,
        /// based on isfollowing property.
        /// </summary>
        private void FollowAction()
        {
            PivotItems[SelectedIndex].IsLoading = true;
            TweetService.FollowOrUnFollow(User.User,
                profile =>
                {
                    #region handle error
                    if (profile.HasError)
                    {
                        PivotItems[SelectedIndex].IsLoading = false;
                        return;
                    }
                    #endregion
                    #region success
                    string message = string.Empty;
                    if (User.IsFollowing)
                        message = "unfollow successfully";
                    else if (User.IsPrivate)
                        message = "request sent successfully";
                    else
                        message = "follow successfully";
                    App.HandleMessage(new ToastMessage
                    {
                        Message = message
                    });
                    TweetService.GetUserProfileDetail(User.User,
                        profileDetail =>
                        {
                            if (profileDetail.HasError)
                                return;
                            this.User = new UserProfileDetailViewModel(profileDetail);
                            ChangeFollowButtonText();
                            (PivotItems[SelectedIndex] as ProfileViewModelBase).UserProfile = User;
                            PivotItems[SelectedIndex].Refresh();
                        });
                    #endregion
                });
        }

        private void EditMyProfileAction()
        {
            (PivotItems[SelectedIndex] as ProfileViewModelBase).EditMyProfile();
        }
        #endregion

        #region private
        private void SwitchAppBar(int selectedIndex)
        {
            #region switch appbar
            if (User.IsMyself)
            {
                if (selectedIndex == 0)
                    State = AppBarState.MyProfileDefault;
                else
                    State = AppBarState.MyProfileWithEdit;
            }
            else
            {
                if (selectedIndex == 0)
                    State = AppBarState.ProfileDefault;
                else
                    State = AppBarState.ProfileWithRefresh;
            }
            #endregion
            (PivotItems[selectedIndex] as ProfileViewModelBase).UserProfile = User;
            base.MainPivot_LoadedPivotItem(selectedIndex);
        }

        private void ChangeFollowButtonText()
        {
            if (User.IsFollowing)
                FollowButtonText = "unfollow";
            else
                FollowButtonText = "follow";
        }
        #endregion
    }
}

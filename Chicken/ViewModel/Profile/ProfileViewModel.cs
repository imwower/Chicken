using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Profile.VM;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModel : PivotViewModelBase
    {
        #region properties
        private UserProfileDetail user;
        public UserProfileDetail User
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
        private bool isInit;
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

        //public ICommand BlocksCommand
        //{
        //    get
        //    {
        //        return new DelegateCommand(BlocksAction);
        //    }
        //}
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
            var selectedUser = IsolatedStorageService.GetObject<User>(PageNameEnum.ProfilePage);
            #region init
            if (!isInit)
            {
                TweetService.GetUserProfileDetail<UserProfileDetail>(selectedUser,
                userProfileDetail =>
                {
                    this.User = userProfileDetail;
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
                        if (selectedIndex == 0)
                        {
                            State = AppBarState.ProfileDefault;
                        }
                        else
                        {
                            State = AppBarState.ProfileWithRefresh;
                        }
                        ChangeFollowButtonText();
                    }
                    #endregion
                    (PivotItems[selectedIndex] as ProfileViewModelBase).UserProfile = User;
                    base.MainPivot_LoadedPivotItem(selectedIndex);
                    isInit = true;
                });
            }
            else
            {
                (PivotItems[selectedIndex] as ProfileViewModelBase).UserProfile = User;
                base.MainPivot_LoadedPivotItem(selectedIndex);
            }
            #endregion
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
            TweetService.FollowOrUnFollow<User>(User,
                userProfile =>
                {
                    List<ErrorMessage> errors = userProfile.Errors;
                    var toastMessage = new ToastMessage();
                    #region handle error
                    if (errors != null && errors.Count != 0)
                    {
                        toastMessage.Message = errors[0].Message;
                        PivotItems[SelectedIndex].ToastMessageHandler(toastMessage);
                        return;
                    }
                    #endregion
                    #region success
                    toastMessage.Message = User.IsFollowing ? "unfollow successfully" : "follow successfully";
                    PivotItems[SelectedIndex].HandleMessage(toastMessage);
                    TweetService.GetUserProfileDetail<UserProfileDetail>(User,
                        userProfileDetail =>
                        {
                            this.User = userProfileDetail;
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

        //private void BlocksAction()
        //{
        //    //TODO
        //} 
        #endregion

        #region private
        private void ChangeFollowButtonText()
        {
            if (User.IsFollowing)
            {
                FollowButtonText = "unfollow";
            }
            else
            {
                FollowButtonText = "follow";
            }
        }
        #endregion
    }
}

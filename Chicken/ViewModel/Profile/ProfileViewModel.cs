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
                if (user.IsFollowing)
                {
                    FollowButtonText = "unfollow";
                }
                else
                {
                    FollowButtonText = "follow";
                }
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
            if (User == null)
            {
                User = IsolatedStorageService.GetObject<User>(PageNameEnum.ProfilePage);
            }
            #region is myself or not
            if (User.Id == App.AuthenticatedUser.Id)
            {
                State = AppBarState.MyProfileDefault;
                User.IsMyself = true;
            }
            #endregion
            #region change appbar menu
            if (selectedIndex != 0)
            {
                State = State + 1;
            }
            #endregion
            (PivotItems[selectedIndex] as ProfileViewModelBase).UserProfile = User;
            base.MainPivot_LoadedPivotItem(selectedIndex);
        }

        private void MentionAction()
        {
            NewTweetModel newTweet = new NewTweetModel
            {
                Type = NewTweetActionType.Mention,
                Text = User.ScreenName + " ",
            };
            PivotItems[SelectedIndex].IsLoading = false;
            NavigationServiceManager.NavigateTo(PageNameEnum.NewTweetPage, newTweet);
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
                    }
                    #endregion
                    #region success
                    else
                    {
                        toastMessage.Message = User.IsFollowing ? "unfollow successfully" : "follow successfully";
                        toastMessage.Complete =
                            () =>
                            {
                                TweetService.GetUserProfileDetail<User>(User.Id,
                                    userProfileDetail =>
                                    {
                                        User = userProfileDetail;
                                        PivotItems[SelectedIndex].Refresh();
                                    });
                            };
                        PivotItems[SelectedIndex].ToastMessageHandler(toastMessage);
                    }
                    #endregion
                });
        }

        private void EditMyProfileAction()
        { }

        private void BlocksAction()
        { }
    }
}

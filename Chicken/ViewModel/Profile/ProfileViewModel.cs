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
        private bool isLoading;
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

        public override void MainPivot_LoadedPivotItem()
        {
            if (IsInit)
            {
                SwitchAppBar();
                return;
            }
            PivotItems[SelectedIndex].IsLoading = true;
            IsolatedStorageService.GetAndDeleteObject<UserProfileDetail>(Const.ProfilePage_UserProfileDetail);
            var file = IsolatedStorageService.GetObject<User>(Const.ProfilePage);
            // if null, use authenticated user
            if (file == null)
                file = App.AuthenticatedUser;
            GetUserProfileDetail(file);
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
            TweetService.FollowOrUnFollow(User,
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
                        message = LanguageHelper.GetString("Toast_Msg_UnfollowSuccessfully");
                    else if (User.IsPrivate)
                        message = LanguageHelper.GetString("Toast_Msg_RequestSentSuccessfully");
                    else
                        message = LanguageHelper.GetString("Toast_Msg_FollowSuccessfully");
                    App.HandleMessage(new ToastMessage
                    {
                        Message = message
                    });
                    TweetService.GetUserProfileDetail(User,
                        profileDetail =>
                        {
                            if (profileDetail.HasError)
                            {
                                isLoading = false;
                                return;
                            }
                            User = profileDetail;
                            IsolatedStorageService.CreateObject(Const.ProfilePage_UserProfileDetail, User);
                            ChangeFollowButtonText();
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
        private void GetUserProfileDetail(User user)
        {
            //sometimes, user is from profile description,
            //should get user detail from service
            //only load once
            if (isLoading)
                return;
            isLoading = true;
            TweetService.GetUserProfileDetail(user,
               profile =>
               {
                   isLoading = false;
                   if (profile.HasError)
                   {
                       PivotItems[SelectedIndex].IsLoading = false;
                       return;
                   }
                   User = profile;
                   if (User.Id == App.AuthenticatedUser.Id)
                       User.IsMyself = true;
                   else
                       ChangeFollowButtonText();
                   IsolatedStorageService.CreateObject(Const.ProfilePage_UserProfileDetail, User);
                   IsInit = true;
                   SwitchAppBar();
               });
        }

        private void SwitchAppBar()
        {
            #region switch appbar
            if (User.IsMyself)
                if (SelectedIndex == 0)
                    State = AppBarState.MyProfileDefault;
                else
                    State = AppBarState.MyProfileWithEdit;
            else
                if (SelectedIndex == 0)
                    State = AppBarState.ProfileDefault;
                else
                    State = AppBarState.ProfileWithRefresh;
            #endregion
            base.MainPivot_LoadedPivotItem();
        }

        private void ChangeFollowButtonText()
        {
            if (User.IsFollowing)
                FollowButtonText = LanguageHelper.GetString("AppBarButton_UnFollow");
            else
                FollowButtonText = LanguageHelper.GetString("AppBarButton_Follow");
        }
        #endregion
    }
}

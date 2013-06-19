using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModelBase : PivotItemViewModelBase
    {
        #region event handler
        public delegate void NewMessageEventHandler();
        public NewMessageEventHandler NewMessageHandler;
        #endregion

        #region properties
        private User userProfile;
        public User UserProfile
        {
            get
            {
                return userProfile;
            }
            set
            {
                userProfile = value;
                RaisePropertyChanged("UserProfile");
            }
        }

        #region for tweets and favorites pivot item
        private ObservableCollection<TweetViewModel> tweetList;
        public ObservableCollection<TweetViewModel> TweetList
        {
            get
            {
                return tweetList;
            }
            set
            {
                tweetList = value;
                RaisePropertyChanged("TweetList");
            }
        }
        #endregion
        #region for following and followers pivot item
        protected string nextCursor = "-1";
        protected string previousCursor;
        private ObservableCollection<UserProfile> userList;
        public ObservableCollection<UserProfile> UserList
        {
            get
            {
                return userList;
            }
            set
            {
                userList = value;
                RaisePropertyChanged("UserList");
            }
        }
        #endregion
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public ProfileViewModelBase()
        {
            ClickHandler = this.ClickAction;
        }

        #region public method
        public virtual void NewMessage()
        {
            if (IsLoading)
            {
                return;
            }
            if (NewMessageHandler != null)
            {
                IsLoading = false;
                NewMessageHandler();
            }
        }

        public virtual void Mention()
        {
            NewTweetModel newTweet = new NewTweetModel
            {
                Type = NewTweetActionType.Mention,
                Text = UserProfile.ScreenName + " ",
            };
            IsLoading = false;
            NavigationServiceManager.NavigateTo(PageNameEnum.NewTweetPage, newTweet);
        }

        /// <summary>
        /// follow or unfollow,
        /// based on isfollowing property.
        /// </summary>
        public virtual void Follow()
        {
            IsLoading = true;
            TweetService.FollowOrUnFollow<User>(UserProfile,
                userProfile =>
                {
                    IsLoading = false;
                    List<ErrorMessage> errors = userProfile.Errors;
                    var toastMessage = new ToastMessage();
                    #region handle error
                    if (errors != null && errors.Count != 0)
                    {
                        toastMessage.Message = errors[0].Message;
                        HandleMessage(toastMessage);
                    }
                    #endregion
                    #region success
                    else
                    {
                        toastMessage.Message = UserProfile.IsFollowing ? "unfollow successfully" : "follow successfully";
                        toastMessage.Complete =
                            () =>
                            {
                                TweetService.GetUserProfileDetail<User>(UserProfile.Id,
                                    userProfileDetail =>
                                    {
                                        UserProfile = userProfileDetail;
                                        Refresh();
                                    });
                            };
                        HandleMessage(toastMessage);
                    }
                    #endregion
                });
        }

        public virtual void EditMyProfile()
        {
            IsLoading = false;
            if (userProfile.IsMyself)
            {
                NavigationServiceManager.NavigateTo(PageNameEnum.EditMyProfilePage);
            }
        }
        #endregion

        #region actions
        private void ClickAction(object parameter)
        {
            var user = parameter as User;
            if (UserProfile.Id == user.Id)
            {
                NavigationServiceManager.ChangeSelectedIndex((int)ProfilePageEnum.ProfileDetail);
            }
            else
            {
                NavigationServiceManager.NavigateTo(PageNameEnum.ProfilePage, user);
            }
        }
        #endregion

        #region protected methods
        #region for following and followers pivot
        protected void RefreshUserProfiles(string userIds)
        {
            TweetService.GetUserProfiles<UserProfileList<UserProfile>>(userIds,
                userProfiles =>
                {
                    for (int i = userProfiles.Count - 1; i >= 0; i--)
                    {
                        UserList.Insert(0, userProfiles[i]);
                    }
                    base.Refreshed();
                });
        }

        protected void LoadUserProfiles(string userIds)
        {
            TweetService.GetUserProfiles<UserProfileList<UserProfile>>(userIds,
                userProfiles =>
                {
                    foreach (var userProfile in userProfiles)
                    {
                        UserList.Add(userProfile);
                    }
                    base.Loaded();
                });
        }
        #endregion
        #endregion
    }
}

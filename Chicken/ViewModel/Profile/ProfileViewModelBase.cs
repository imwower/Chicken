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

        #region for tweets and favorites pivot
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

        #region for following and followers pivot
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
        public virtual void Mention()
        {
            if (IsLoading)
            {
                return;
            }
            NewTweetModel newTweet = new NewTweetModel
            {
                Type = NewTweetActionType.Mention,
                Text = UserProfile.ScreenName + " ",
            };
            IsLoading = false;
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.NewTweetPage, newTweet);
        }

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
        #endregion

        #region private method
        private void ClickAction(object parameter)
        {
            var user = parameter as User;
            if (UserProfile.Id == user.Id)
            {
                NavigationServiceManager.ChangeSelectedIndex((int)Const.ProfilePageEnum.ProfileDetail);
            }
            else
            {
                NavigationServiceManager.NavigateTo(Const.PageNameEnum.ProfilePage, user);
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

using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Profile.Base;
using System.Collections.Generic;
using Chicken.Common;
using System.Collections.ObjectModel;
using Chicken.Model;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModelBase : ViewModelBase
    {
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

        #region for tweets and favourites pivot
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
        private ObservableCollection<UserProfileViewModel> userList;
        public ObservableCollection<UserProfileViewModel> UserList
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
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        public override void Click(object parameter)
        {
            if (UserId == parameter.ToString())
            {
                NavigationServiceManager.ChangeSelectedIndex((int)Const.ProfilePageEnum.ProfileDetail);
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>(1);
                parameters.Add(Const.USER_ID, parameter);
                NavigationServiceManager.NavigateTo(Const.ProfilePage, parameters);
            }
        }

        #region protected methods
        #region for following and followers pivot
        protected void RefreshUserProfiles(string userIds)
        {
            TweetService.GetUserProfiles<List<UserProfile>>(userIds,
                userProfiles =>
                {
                    userProfiles.Reverse();
                    foreach (var userProfile in userProfiles)
                    {
                        UserList.Insert(0, new UserProfileViewModel(userProfile));
                    }
                });
        }

        protected void LoadUserProfiles(string userIds)
        {
            TweetService.GetUserProfiles<List<UserProfile>>(userIds,
                userProfiles =>
                {
                    foreach (var userProfile in userProfiles)
                    {
                        UserList.Add(new UserProfileViewModel(userProfile));
                    }
                });
        }
        #endregion
        #endregion
    }
}

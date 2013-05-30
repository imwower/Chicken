using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Profile.Base;

namespace Chicken.ViewModel.Profile.VM
{
    public class UserFollowingViewModel : ProfileViewModelBase
    {
        #region properties
        private string nextCursor = "-1";
        private string previousCursor;
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

        public UserFollowingViewModel()
        {
            Header = "Following";
            UserList = new ObservableCollection<UserProfileViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
        }

        private void RefreshAction()
        {
            if (previousCursor == "0")
            {
                return;
            }
            var parameters = TwitterHelper.GetDictionary();
            if (!string.IsNullOrEmpty(previousCursor))
            {
                parameters.Add(Const.CURSOR, previousCursor);
            }
            TweetService.GetFollowingIds<UserIdList>(UserId,
                userIdList =>
                {
                    if (userIdList == null || userIdList.UserIds == null)
                    {
                        return;
                    }
                    else
                    {
                        nextCursor = userIdList.NextCursor;
                        previousCursor = userIdList.PreviousCursor;
                        string ids = string.Join(",", userIdList.UserIds);
                        RefreshUserProfiles(ids);
                    }
                }, parameters);
        }

        private void RefreshUserProfiles(string userIds)
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

        private void LoadAction()
        {
            if (nextCursor == "0")
            {
                return;
            }
            var parameters = TwitterHelper.GetDictionary();
            if (!string.IsNullOrEmpty(nextCursor))
            {
                parameters.Add(Const.CURSOR, nextCursor);
            }
            TweetService.GetFollowingIds<UserIdList>(UserId,
                userIdList =>
                {
                    if (userIdList == null || userIdList.UserIds == null)
                    {
                        return;
                    }
                    else
                    {
                        nextCursor = userIdList.NextCursor;
                        previousCursor = userIdList.PreviousCursor;
                        string ids = string.Join(",", userIdList.UserIds);
                        LoadUserProfiles(ids);
                    }
                }, parameters);
        }

        private void LoadUserProfiles(string userIds)
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
    }
}

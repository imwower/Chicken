using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Profile.Base;

namespace Chicken.ViewModel.Profile.VM
{
    public class UserFollowersViewModel : ProfileViewModelBase
    {
        public UserFollowersViewModel()
        {
            Header = "Followers";
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
            TweetService.GetFollowerIds<UserIdList>(UserId,
                result =>
                {
                    UserIdList userIdList = result as UserIdList;
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
            TweetService.GetFollowerIds<UserIdList>(UserId,
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
    }
}

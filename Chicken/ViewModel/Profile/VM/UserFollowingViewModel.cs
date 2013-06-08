using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;

namespace Chicken.ViewModel.Profile.VM
{
    public class UserFollowingViewModel : ProfileViewModelBase
    {
        public UserFollowingViewModel()
        {
            Header = "Following";
            UserList = new ObservableCollection<UserProfile>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
        }

        private void RefreshAction()
        {
            if (previousCursor == "0")
            {
                base.Refreshed();
                return;
            }
            var parameters = TwitterHelper.GetDictionary();
            if (!string.IsNullOrEmpty(previousCursor))
            {
                parameters.Add(Const.CURSOR, previousCursor);
            }
            TweetService.GetFollowingIds<UserIdList>(User.Id,
                userIdList =>
                {
                    if (userIdList == null || userIdList.UserIds == null || userIdList.UserIds.Count == 0)
                    {
                        base.Refreshed();
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
                base.Loaded();
                return;
            }
            var parameters = TwitterHelper.GetDictionary();
            if (!string.IsNullOrEmpty(nextCursor))
            {
                parameters.Add(Const.CURSOR, nextCursor);
            }
            TweetService.GetFollowingIds<UserIdList>(User.Id,
                userIdList =>
                {
                    if (userIdList == null || userIdList.UserIds == null || userIdList.UserIds.Count == 0)
                    {
                        base.Loaded();
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

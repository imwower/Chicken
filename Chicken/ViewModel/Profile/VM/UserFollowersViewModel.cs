using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;

namespace Chicken.ViewModel.Profile.VM
{
    public class UserFollowersViewModel : ProfileViewModelBase
    {
        public UserFollowersViewModel()
        {
            Header = "Followers";
            UserList = new ObservableCollection<UserProfile>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
        }

        private void RefreshAction()
        {
            if (!CheckIfFollowingPrivate())
            {
                base.Refreshed();
                return;
            }
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
            TweetService.GetFollowerIds<UserIdList>(UserProfile.Id,
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
            TweetService.GetFollowerIds<UserIdList>(UserProfile.Id,
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

using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Status.VM
{
    public class StatusRetweetsViewModel : StatusViewModelBase
    {
        public StatusRetweetsViewModel()
        {
            Header = "Retweets";
            UserList = new ObservableCollection<UserProfileViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = LoadAction;
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
            TweetService.GetStatusRetweetIds<UserIdList>(Tweet.Id,
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
            TweetService.GetStatusRetweetIds<UserIdList>(Tweet.Id,
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

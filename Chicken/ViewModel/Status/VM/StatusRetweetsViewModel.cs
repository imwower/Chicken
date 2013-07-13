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
            UserList = new ObservableCollection<UserProfileViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = LoadAction;
        }

        private void RefreshAction()
        {
            if (!CheckIfLoaded())
                return;
            if (previousCursor == "0")
            {
                base.Refreshed();
                return;
            }
            #region parameter
            var parameters = TwitterHelper.GetDictionary();
            if (!string.IsNullOrEmpty(previousCursor))
                parameters.Add(Const.CURSOR, previousCursor);
            #endregion
            TweetService.GetStatusRetweetIds(Tweet.Id,
                userIdList =>
                {
                    #region handle error
                    if (userIdList.HasError)
                    {
                        base.Refreshed();
                        return;
                    }
                    #endregion
                    #region no retweets yet
                    if (userIdList.UserIds.Count == 0)
                    {
                        App.HandleMessage(new ToastMessage
                        {
                            Message = LanguageHelper.GetString("Toast_Msg_NoNewRetweets"),
                        });
                        base.Refreshed();
                        return;
                    }
                    #endregion
                    nextCursor = userIdList.NextCursor;
                    previousCursor = userIdList.PreviousCursor;
                    string ids = string.Join(",", userIdList.UserIds);
                    RefreshUserProfiles(ids);
                }, parameters);
        }

        private void LoadAction()
        {
            if (nextCursor == "0")
            {
                base.Loaded();
                return;
            }
            #region parameter
            var parameters = TwitterHelper.GetDictionary();
            if (!string.IsNullOrEmpty(nextCursor))
            {
                parameters.Add(Const.CURSOR, nextCursor);
            }
            #endregion
            TweetService.GetStatusRetweetIds(Tweet.Id,
                userIdList =>
                {
                    if (userIdList.HasError)
                    {
                        base.Loaded();
                        return;
                    }
                    if (userIdList.UserIds.Count == 0)
                    {
                        App.HandleMessage(new ToastMessage
                        {
                            Message = LanguageHelper.GetString("Toast_Msg_NoMoreRetweets"),
                        });
                        base.Loaded();
                        return;
                    }
                    nextCursor = userIdList.NextCursor;
                    previousCursor = userIdList.PreviousCursor;
                    string ids = string.Join(",", userIdList.UserIds);
                    LoadUserProfiles(ids);
                }, parameters);
        }
    }
}

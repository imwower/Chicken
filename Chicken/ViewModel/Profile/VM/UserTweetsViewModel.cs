using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Profile.VM
{
    public class UserTweetsViewModel : ProfileViewModelBase
    {
        public UserTweetsViewModel()
        {
            TweetList = new ObservableCollection<TweetViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
        }

        #region actions
        private void RefreshAction()
        {
            if (!CheckIfFollowingPrivate())
            {
                return;
            }
            #region parameter
            string sinceId = string.Empty;
            var parameters = TwitterHelper.GetDictionary();
            if (TweetList.Count != 0)
            {
                sinceId = TweetList[0].Id;
                parameters.Add(Const.SINCE_ID, sinceId);
            }
            #endregion
            TweetService.GetUserTweets(UserProfile.User,
                tweets =>
                {
                    if (!tweets.HasError)
                    {
                        #region no new tweets yet
                        if (tweets.Count == 0)
                        {
                            App.HandleMessage(new ToastMessage
                            {
                                Message = LanguageHelper.GetString("Toast_Msg_NoNewTweets"),
                            });
                        }
                        #endregion
                        #region add
                        else
                        {
                            for (int i = tweets.Count - 1; i >= 0; i--)
                            {
#if !LOCAL
                                if (sinceId == tweets[i].Id)
                                    continue;
#endif
                                TweetList.Insert(0, new TweetViewModel(tweets[i]));
                                if (TweetList.Count >= Const.DEFAULT_COUNT_VALUE)
                                    TweetList.RemoveAt(TweetList.Count - 1);
                            }
                        }
                        #endregion
                    }
                    base.Refreshed();
                }, parameters);
        }

        private void LoadAction()
        {
            if (TweetList.Count == 0)
            {
                base.Loaded();
                return;
            }
            #region parameter
            string maxId = TweetList[TweetList.Count - 1].Id;
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.MAX_ID, maxId);
            #endregion
            TweetService.GetUserTweets(UserProfile.User,
                tweets =>
                {
                    if (!tweets.HasError)
                    {
                        #region no more tweets yet
                        if (tweets.Count == 0)
                        {
                            App.HandleMessage(new ToastMessage
                            {
                                Message = LanguageHelper.GetString("Toast_Msg_NoMoreTweets"),
                            });
                        }
                        #endregion
                        #region add
                        else
                        {
                            foreach (var tweet in tweets)
                            {
#if !LOCAL
                                if (maxId != tweet.Id)
#endif
                                TweetList.Add(new TweetViewModel(tweet));
                            }
                        }
                        #endregion
                    }
                    base.Loaded();
                }, parameters);

        }
        #endregion
    }
}

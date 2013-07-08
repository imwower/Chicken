using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
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
            ItemClickHandler = this.ItemClickAction;
        }

        #region actions
        private void RefreshAction()
        {
            if (!CheckIfFollowingPrivate())
            {
                base.Refreshed();
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
#if !LOCAL
                            if (string.Compare(sinceId, tweets[0].Id) == -1)
                                TweetList.Clear();
#endif
                            for (int i = tweets.Count - 1; i >= 0; i--)
                            {
#if !LOCAL
                                if (sinceId != tweets[i].Id)
#endif
                                TweetList.Insert(0, new TweetViewModel(tweets[i]));
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

        private void ItemClickAction(object parameter)
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(PageNameEnum.StatusPage, parameter);
        }
        #endregion
    }
}

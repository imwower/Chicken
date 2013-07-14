using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Home.VM
{
    public class MentionsViewModel : HomeViewModelBase
    {
        public MentionsViewModel()
        {
            TweetList = new ObservableCollection<TweetViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
        }

        #region actions
        private void RefreshAction()
        {
            #region init from cache
            if (InitFromCache())
                return;
            #endregion
            #region parameter
            string sinceId = string.Empty;
            var parameters = TwitterHelper.GetDictionary();
            if (TweetList.Count != 0)
            {
                sinceId = TweetList[0].Id;
                parameters.Add(Const.SINCE_ID, sinceId);
            }
            #endregion
            #region request
            TweetService.GetMentions(
                tweets =>
                {
                    if (!tweets.HasError)
                    {
                        #region no new mentions
                        if (tweets.Count == 0)
                        {
                            App.HandleMessage(new ToastMessage
                            {
                                Message = LanguageHelper.GetString("Toast_Msg_NoNewMentions"),
                            });
                        }
                        #endregion
                        #region add
                        else
                        {
#if !LOCAL
                            if (sinceId == tweets[tweets.Count - 1].Id)
                                tweets.RemoveAt(tweets.Count - 1);
#endif
                            for (int i = tweets.Count - 1; i >= 0; i--)
                            {
                                TweetList.Insert(0, new TweetViewModel(tweets[i]));
                                if (TweetList.Count >= Const.DEFAULT_COUNT_VALUE)
                                    TweetList.RemoveAt(TweetList.Count - 1);
                            }
                            IsolatedStorageService.AddMentions(tweets);
                        }
                        #endregion
                    }
                    base.Refreshed();
                }, parameters);
            #endregion
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
            TweetService.GetMentions(
                tweets =>
                {
                    if (!tweets.HasError)
                    {
                        #region no more mentions
                        if (tweets.Count == 0)
                        {
                            App.HandleMessage(new ToastMessage
                            {
                                Message = LanguageHelper.GetString("Toast_Msg_NoMoreMentions"),
                            });
                        }
                        #endregion
                        #region add
                        else
                        {
#if !LOCAL
                            if (maxId == tweets[0].Id)
                                tweets.RemoveAt(0);
#endif
                            foreach (var tweet in tweets)
                            {
                                TweetList.Add(new TweetViewModel(tweet));
                            }
                        }
                        #endregion
                    }
                    base.Loaded();
                }, parameters);
        }
        #endregion

        #region private method
        /// <summary>
        /// if cache existes, init, and return true;
        /// </summary>
        /// <returns></returns>
        private bool InitFromCache()
        {
            if (IsInited)
                return false;
            var list = IsolatedStorageService.GetMentions();
            if (list != null && list.Count != 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                    TweetList.Insert(0, new TweetViewModel(list[i]));
                base.Refreshed();
                return true;
            }
            return false;
        }
        #endregion
    }
}

﻿using System.Collections.ObjectModel;
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

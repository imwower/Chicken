using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Home.VM
{
    public class MentionsViewModel : HomeViewModelBase
    {
        public MentionsViewModel()
        {
            Header = "Mentions";
            TweetList = new ObservableCollection<TweetViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
        }

        private void RefreshAction()
        {
            string sinceId = string.Empty;
            var parameters = TwitterHelper.GetDictionary();
            if (TweetList.Count != 0)
            {
                sinceId = TweetList[0].Id;
                parameters.Add(Const.SINCE_ID, sinceId);
            }
            TweetService.GetMentions<TweetList<Tweet>>(
                tweets =>
                {
                    if (tweets != null && tweets.Count != 0)
                    {
#if !DEBUG
                        if (string.Compare(sinceId, tweets[0].Id) == -1)
                        {
                            TweetList.Clear();
                        }
#endif
                        for (int i = tweets.Count - 1; i >= 0; i--)
                        {
#if !DEBUG
                            if (sinceId != tweets[i].Id)
#endif
                            {
                                TweetList.Insert(0, new TweetViewModel(tweets[i]));
                            }
                        }
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
            else
            {
                string maxId = TweetList[TweetList.Count - 1].Id;
                var parameters = TwitterHelper.GetDictionary();
                parameters.Add(Const.MAX_ID, maxId);
                TweetService.GetMentions<List<Tweet>>(
                    tweets =>
                    {
                        foreach (var tweet in tweets)
                        {
#if !DEBUG
                            if (maxId != tweet.Id)
#endif
                            {
                                TweetList.Add(new TweetViewModel(tweet));
                            }
                        }
                        base.Loaded();
                    }, parameters);
            }
        }
    }
}

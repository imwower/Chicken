using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Home.Base;
using System;

namespace Chicken.ViewModel.Home.VM
{
    public class HomeViewModel : HomeViewModelBase
    {
        public HomeViewModel()
        {
            Header = "Home";
            TweetList = new ObservableCollection<TweetViewModel>();
            RefreshHandler = RefreshAction;
        }

        private void RefreshAction(object sender, EventArgs e)
        {
            string sinceId = string.Empty;
            var parameters = TwitterHelper.GetDictionary();
            if (TweetList.Count != 0)
            {
                sinceId = TweetList[0].Id;
                parameters.Add(Const.SINCE_ID, sinceId);
            }
            TweetService.GetTweets<List<Tweet>>(
                tweets =>
                {
                    if (tweets != null && tweets.Count != 0)
                    {
                        tweets.Reverse();
#if HTTP
                        if (string.Compare(sinceId, tweets[0].Id) == -1)
                        {
                            TweetList.Clear();
                        } 
#endif
                        foreach (var tweet in tweets)
                        {
#if HTTP
                            if (sinceId != tweet.Id) 
#endif
                            {
                                TweetList.Insert(0, new TweetViewModel(tweet));
                            }
                        }
                    }
                    //base.Refreshed();
                }, parameters);
        }

        public override void Load()
        {
            if (IsLoading)
            {
                return;
            }
            if (TweetList.Count == 0)
            {
                base.Load();
                return;
            }
            else
            {
                IsLoading = true;
                string maxId = TweetList[TweetList.Count - 1].Id;
                var parameters = TwitterHelper.GetDictionary();
                parameters.Add(Const.MAX_ID, maxId);
                TweetService.GetTweets<List<Tweet>>(
                    tweets =>
                    {
                        foreach (var tweet in tweets)
                        {
                            if (maxId != tweet.Id)
                            {
                                TweetList.Add(new TweetViewModel(tweet));
                            }
                        }
                        base.Load();
                    }, parameters);
            }
        }
    }
}

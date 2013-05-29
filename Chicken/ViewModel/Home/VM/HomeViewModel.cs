using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Home.Base;
using Chicken.Common;

namespace Chicken.ViewModel.Home.VM
{
    public class HomeViewModel : HomeViewModelBase
    {
        public HomeViewModel()
        {
            Header = "Home";
            TweetList = new ObservableCollection<TweetViewModel>();
        }

        public override void Refresh()
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
                        if (string.Compare(sinceId, tweets[0].Id) == -1)
                        {
                            TweetList.Clear();
                        }
                        foreach (var tweet in tweets)
                        {
                            if (sinceId != tweet.Id)
                            {
                                TweetList.Insert(0, new TweetViewModel(tweet));
                            }
                        }
                    }
                    base.Refresh();
                }, parameters);
        }

        public override void Load()
        {
            if (TweetList.Count == 0)
            {
                base.Load();
                return;
            }
            else
            {
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

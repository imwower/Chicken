using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Home.Base;

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
            TweetService.GetLastedTweets<List<Tweet>>(
                tweets =>
                {
                    tweets.Reverse();
                    foreach (var tweet in tweets)
                    {
                        TweetList.Insert(0, new TweetViewModel(tweet));
                    }
                    base.Refreshed();
                });
        }

        public override void Load()
        {
            TweetService.GetLastedTweets<List<Tweet>>(
                tweets =>
                {
                    foreach (var tweet in tweets)
                    {
                        TweetList.Add(new TweetViewModel(tweet));
                    }
                    base.Loaded();
                });
        }
    }
}

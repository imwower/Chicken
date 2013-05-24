using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Chicken.Model;
using System.Collections.Generic;
using Chicken.Service;

namespace Chicken.ViewModel.Home
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

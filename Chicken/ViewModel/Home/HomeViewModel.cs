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
        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        public HomeViewModel()
        {
            Header = "Home";
            TweetList = new ObservableCollection<TweetViewModel>();
        }

        public override void Refresh()
        {
            var tweets = TweetService.GetNewTweets();
            tweets.Reverse();
            foreach (var tweet in tweets)
            {
                tweet.Text = TweetList.Count + tweet.Text;
                TweetList.Insert(0, new TweetViewModel(tweet));
            }
        }

        public override void Load()
        {
            var tweets = TweetService.GetNewTweets();
            foreach (var tweet in tweets)
            {
                TweetList.Add(new TweetViewModel(tweet));
            }
        }
    }
}

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

namespace Chicken.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        public HomeViewModel()
            : base()
        {
            Header = "Home";
            GetNewTweets();
        }

        public override void GetNewTweets()
        {
            var tweets = TweetService.GetNewTweets();
            tweets.Reverse();
            foreach (var tweet in tweets)
            {
                TweetList.Insert(0, new TweetViewModel(tweet));
            }
        }

        public void GetOldTweets()
        {
            var tweets = TweetService.GetNewTweets();
            foreach (var tweet in tweets)
            {
                TweetList.Add(new TweetViewModel(tweet));
            }
        }
    }
}

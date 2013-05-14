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
        #region properties

        private ObservableCollection<TweetViewModel> homeTimeLineTweets = new ObservableCollection<TweetViewModel>();
        public ObservableCollection<TweetViewModel> HomeTimeLineTweets
        {
            get
            {
                return homeTimeLineTweets;
            }
            set
            {
                if (value != homeTimeLineTweets)
                {
                    homeTimeLineTweets = value;
                    RaisePropertyChanged("HomeTimeLineTweets");
                }
            }
        }

        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        public HomeViewModel(DependencyObject container)
            : base(container)
        {
            Header = "Home";
            GetHomeLineTweets();
            HandleVisualStatueChangedPullUpEvent += HandleVisualStatueChangedPullUp;
        }

        public void GetHomeLineTweets()
        {
            var tweets = TweetService.GetHomeLineTweets();
            var tweetViewModels = new ObservableCollection<TweetViewModel>();
            foreach (var tweet in tweets)
            {
                tweetViewModels.Add(new TweetViewModel(tweet));
            }
            this.homeTimeLineTweets = tweetViewModels;
        }

        public void AppendOldTweets()
        {
            var tweets = TweetService.GetHomeLineTweets();
            foreach (var tweet in tweets)
            {
                this.HomeTimeLineTweets.Add(new TweetViewModel(tweet));
            }
        }

        public void HandleVisualStatueChangedPullUp(object sender, VisualStateChangedEventArgs e)
        {
            AppendOldTweets();
        }
    }
}

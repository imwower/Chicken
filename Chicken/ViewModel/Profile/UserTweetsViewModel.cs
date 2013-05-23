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
using Chicken.Service;
using System.Collections.ObjectModel;
using Chicken.ViewModel.Home;

namespace Chicken.ViewModel.Profile
{
    public class UserTweetsViewModel : ProfileViewModelBase
    {
        #region properties
        private ObservableCollection<TweetViewModel> tweetList;
        public ObservableCollection<TweetViewModel> TweetList
        {
            get
            {
                return tweetList;
            }
            set
            {
                tweetList = value;
                RaisePropertyChanged("TweetList");
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        public UserTweetsViewModel()
        {
            Header = "Tweets";
            TweetList = new ObservableCollection<TweetViewModel>();
        }

        public override void Refresh()
        {
            var tweets = TweetService.GetUserTweets(UserId);
            tweets.Reverse();
            foreach (var tweet in tweets)
            {
                tweet.Text = TweetList.Count + tweet.Text;
                TweetList.Insert(0, new TweetViewModel(tweet));
            }
            base.Refreshed();
        }
    }
}

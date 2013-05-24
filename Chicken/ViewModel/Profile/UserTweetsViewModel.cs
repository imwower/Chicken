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
using System.Collections.Generic;
using Chicken.Common;

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

        public UserTweetsViewModel()
        {
            Header = "Tweets";
            TweetList = new ObservableCollection<TweetViewModel>();
        }

        public override void Refresh()
        {
            base.Refresh();
            var tweets = TweetService.GetUserTweets(UserId);
            tweets.Reverse();
            foreach (var tweet in tweets)
            {
                tweet.Text = TweetList.Count + tweet.Text;
                TweetList.Insert(0, new TweetViewModel(tweet));
            }
            base.Refreshed();
        }

        public void Click(object parameter)
        {
            IsLoading = false;
            Dictionary<string, object> parameters = new Dictionary<string, object>(1);
            parameters.Add(TwitterHelper.USER_ID, parameter);
            string uri = TwitterHelper.GenerateRelativeUri(TwitterHelper.ProfilePage, parameters);
            TwitterHelper.NavigateTo(uri);
        }
    }
}

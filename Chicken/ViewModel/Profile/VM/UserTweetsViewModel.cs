using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Service;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Profile.VM
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

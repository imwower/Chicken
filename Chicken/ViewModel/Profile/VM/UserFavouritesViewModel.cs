using System.Collections.ObjectModel;
using Chicken.ViewModel.Home.Base;
using Chicken.Common;
using System.Collections.Generic;

namespace Chicken.ViewModel.Profile.VM
{
    public class UserFavouritesViewModel : ProfileViewModelBase
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

        public UserFavouritesViewModel()
        {
            Header = "Favourites";
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
    }
}

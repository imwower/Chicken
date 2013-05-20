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
using System.Collections.ObjectModel;
using Chicken.Service;

namespace Chicken.ViewModel
{
    public class MentionsViewModel : BaseViewModel
    {
        #region properties

        private ObservableCollection<TweetViewModel> mentionTweets = new ObservableCollection<TweetViewModel>();
        public ObservableCollection<TweetViewModel> MentionTweets
        {
            get
            {
                return mentionTweets;
            }
            set
            {
                if (value != mentionTweets)
                {
                    mentionTweets = value;
                    RaisePropertyChanged("MentionTweets");
                }
            }
        }

        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        public MentionsViewModel()
            : base()
        {
            Header = "Mentions";
        }

        public override void Refresh()
        {
            var tweets = TweetService.GetNewMentions();
            tweets.Reverse();
            foreach (var tweet in tweets)
            {
                tweet.Text = TweetList.Count + tweet.Text;
                TweetList.Insert(0, new TweetViewModel(tweet));
            }
        }

        public override void Load()
        {
            var tweets = TweetService.GetOldMentions();
            foreach (var tweet in tweets)
            {
                TweetList.Add(new TweetViewModel(tweet));
            }
        }
    }
}

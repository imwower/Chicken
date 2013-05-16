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

        //public MentionsViewModel(DependencyObject container)
        //    : base(container)
        //{
        //    Header = "Mentions";
        //    GetMentionTweets();
        //    HandleVisualStatueChangedPullUpEvent += HandleVisualStatueChangedPullUp;
        //}

        //public void GetOldTweets()
        //{
        //    var tweets = TweetService.GetOldTweets();
        //    var tweetViewModels = new ObservableCollection<TweetViewModel>();
        //    foreach (var tweet in tweets)
        //    {
        //        tweetViewModels.Add(new TweetViewModel(tweet));
        //    }
        //    this.mentionTweets = tweetViewModels;
        //}

        public void AppendOldTweets()
        {
            var tweets = TweetService.GetOldTweets();
            foreach (var tweet in tweets)
            {
                this.MentionTweets.Insert(0, new TweetViewModel(tweet));
            }
        }

        public void HandleVisualStatueChangedPullUp(object sender, VisualStateChangedEventArgs e)
        {
            AppendOldTweets();
        }

    }
}

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

namespace Chicken.ViewModel.Home
{
    public class MentionsViewModel : HomeViewModelBase
    {
        public MentionsViewModel()
        {
            Header = "Mentions";
            TweetList = new ObservableCollection<TweetViewModel>();
        }

        public override void Refresh()
        {
            base.Refresh();
            var tweets = TweetService.GetNewMentions();
            tweets.Reverse();
            foreach (var tweet in tweets)
            {
                tweet.Text = TweetList.Count + tweet.Text;
                TweetList.Insert(0, new TweetViewModel(tweet));
            }
            base.Refreshed();
        }

        public override void Load()
        {
            base.Load();
            var tweets = TweetService.GetOldMentions();
            foreach (var tweet in tweets)
            {
                TweetList.Add(new TweetViewModel(tweet));
            }
            base.Loaded();
        }
    }
}

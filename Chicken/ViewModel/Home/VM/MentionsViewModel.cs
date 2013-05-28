using System.Collections.ObjectModel;
using Chicken.Service;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Home.VM
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
            //base.Refresh();
            var tweets = TweetService.GetNewMentions();
            tweets.Reverse();
            foreach (var tweet in tweets)
            {
                tweet.Text = TweetList.Count + tweet.Text;
                TweetList.Insert(0, new TweetViewModel(tweet));
            }
            base.Refresh();
        }

        public override void Load()
        {
            base.Load();
            var tweets = TweetService.GetOldMentions();
            foreach (var tweet in tweets)
            {
                TweetList.Add(new TweetViewModel(tweet));
            }
            base.Load();
        }
    }
}

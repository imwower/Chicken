using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Home.VM
{
    public class HomeViewModel : HomeViewModelBase
    {
        public HomeViewModel()
        {
            Header = "Home";
            TweetList = new ObservableCollection<TweetViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
        }

        private void RefreshAction()
        {
            string sinceId = string.Empty;
            var parameters = TwitterHelper.GetDictionary();
            if (TweetList.Count != 0)
            {
                sinceId = TweetList[0].Id;
                parameters.Add(Const.SINCE_ID, sinceId);
            }
            TweetService.GetTweets<TweetList>(
                list =>
                {
                    if (list != null && list.Count != 0)
                    {
#if !LOCAL
                        if (string.Compare(sinceId, list[0].Id) == -1)
                        {
                            TweetList.Clear();
                        }
#endif
                        for (int i = list.Count - 1; i >= 0; i--)
                        {
#if !LOCAL
                            if (sinceId != list[i].Id)
#endif
                            {
                                TweetList.Insert(0, new TweetViewModel(list[i]));
                            }
                        }
                    }
                    base.Refreshed();
                }, parameters);
        }

        private void LoadAction()
        {
            if (TweetList.Count == 0)
            {
                base.Loaded();
                return;
            }
            else
            {
                string maxId = TweetList[TweetList.Count - 1].Id;
                var parameters = TwitterHelper.GetDictionary();
                parameters.Add(Const.MAX_ID, maxId);
                TweetService.GetTweets<TweetList>(
                    tweetList =>
                    {
                        if (tweetList != null && tweetList.Count != 0)
                        {
                            foreach (var tweet in tweetList)
                            {
#if !LOCAL
                                if (maxId != tweet.Id)
#endif
                                {
                                    TweetList.Add(new TweetViewModel(tweet));
                                }
                            }
                        }
                        base.Loaded();
                    }, parameters);
            }
        }
    }
}

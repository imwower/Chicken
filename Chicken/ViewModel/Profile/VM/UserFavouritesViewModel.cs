using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Profile.VM
{
    public class UserFavoritesViewModel : ProfileViewModelBase
    {
        public UserFavoritesViewModel()
        {
            Header = "Favorites";
            TweetList = new ObservableCollection<TweetViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
            ItemClickHandler = this.ItemClickAction;
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
            TweetService.GetUserFavorites<TweetList<Tweet>>(UserId,
                tweets =>
                {
                    if (tweets != null && tweets.Count != 0)
                    {
#if !DEBUG
                        if (string.Compare(sinceId, tweets[0].Id) == -1)
                        {
                            TweetList.Clear();
                        }
#endif
                        for (int i = tweets.Count - 1; i >= 0; i--)
                        {
#if !DEBUG
                            if (sinceId != tweets[i].Id)
#endif
                            {
                                TweetList.Insert(0, new TweetViewModel(tweets[i]));
                            }
                        }
                    }
                }, parameters);
        }

        private void LoadAction()
        {
            if (TweetList.Count == 0)
            {
                return;
            }
            else
            {
                string maxId = TweetList[TweetList.Count - 1].Id;
                var parameters = TwitterHelper.GetDictionary();
                parameters.Add(Const.MAX_ID, maxId);
                TweetService.GetUserFavorites<TweetList<Tweet>>(UserId,
                    tweets =>
                    {
                        foreach (var tweet in tweets)
                        {
#if !DEBUG
                            if (maxId != tweet.Id)
#endif
                            {
                                TweetList.Add(new TweetViewModel(tweet));
                            }
                        }
                    }, parameters);
            }
        }

        private void ItemClickAction(object parameter)
        {
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.StatusPage, parameter);
        }
    }
}

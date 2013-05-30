using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Profile.VM
{
    public class UserFavouritesViewModel : ProfileViewModelBase
    {
        public UserFavouritesViewModel()
        {
            Header = "Favourites";
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
            TweetService.GetUserFavourites<List<Tweet>>(UserId,
                tweets =>
                {
                    if (tweets != null && tweets.Count != 0)
                    {
                        tweets.Reverse();
                        if (string.Compare(sinceId, tweets[0].Id) == -1)
                        {
                            TweetList.Clear();
                        }
                        foreach (var tweet in tweets)
                        {
                            if (sinceId != tweet.Id)
                            {
                                TweetList.Insert(0, new TweetViewModel(tweet));
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
                TweetService.GetUserFavourites<List<Tweet>>(UserId,
                    tweets =>
                    {
                        foreach (var tweet in tweets)
                        {
                            if (maxId != tweet.Id)
                            {
                                TweetList.Add(new TweetViewModel(tweet));
                            }
                        }
                    }, parameters);
            }
        }

        private void ItemClickAction(object parameter)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.ID, parameter);
            NavigationServiceManager.NavigateTo(Const.StatusPage, parameters);
        }
    }
}

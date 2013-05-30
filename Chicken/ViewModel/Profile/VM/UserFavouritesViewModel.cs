using System.Collections.ObjectModel;
using Chicken.ViewModel.Home.Base;
using Chicken.Common;
using System.Collections.Generic;
using Chicken.Model;
using Chicken.Service;

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
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;
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
                    base.Refreshed();
                }, parameters);
        }

        public override void Load()
        {
            if (IsLoading)
            {
                return;
            }
            if (TweetList.Count == 0)
            {
                base.Load();
                return;
            }
            else
            {
                IsLoading = true;
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
                        base.Load();
                    }, parameters);
            }
        }

        public override void ItemClick(object parameter)
        {
            IsLoading = false;
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.ID, parameter);
            NavigationServiceManager.NavigateTo(Const.StatusPage, parameters);
        }
    }
}

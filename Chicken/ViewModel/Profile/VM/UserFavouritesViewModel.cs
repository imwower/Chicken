using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Base;

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
            if (!CheckIfFollowingPrivate())
            {
                base.Refreshed();
                return;
            }
            string sinceId = string.Empty;
            var parameters = TwitterHelper.GetDictionary();
            if (TweetList.Count != 0)
            {
                sinceId = TweetList[0].Id;
                parameters.Add(Const.SINCE_ID, sinceId);
            }
            TweetService.GetUserFavorites<TweetList>(UserProfile.User,
                tweets =>
                {
                    if (tweets != null && tweets.Count != 0)
                    {
#if !LOCAL
                        if (string.Compare(sinceId, tweets[0].Id) == -1)
                        {
                            TweetList.Clear();
                        }
#endif
                        for (int i = tweets.Count - 1; i >= 0; i--)
                        {
#if !LOCAL
                            if (sinceId != tweets[i].Id)
#endif
                            {
                                TweetList.Insert(0, new TweetViewModel(tweets[i]));
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
                TweetService.GetUserFavorites<TweetList>(UserProfile.User,
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
                        base.Loaded();
                    }, parameters);
            }
        }

        private void ItemClickAction(object parameter)
        {
            NavigationServiceManager.NavigateTo(PageNameEnum.StatusPage, parameter);
        }
    }
}

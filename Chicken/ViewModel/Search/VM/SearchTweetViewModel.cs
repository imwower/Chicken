using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Search.VM
{
    public class SearchTweetViewModel : SearchViewModelBase
    {
        public SearchTweetViewModel()
        {
            TweetList = new ObservableCollection<TweetViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
        }

        #region override
        public override void Search()
        {
            if (IsLoading)
                return;
            IsLoading = true;
            RefreshAction();
        }
        #endregion

        #region actions
        private void RefreshAction()
        {
            #region check
            if (!CheckAndGetSearchQuery())
                return;
            #endregion
            #region parameter
            string sinceId = string.Empty;
            var parameters = TwitterHelper.GetDictionary();
            if (TweetList.Count != 0)
            {
                sinceId = TweetList[0].Id;
                parameters.Add(Const.SINCE_ID, sinceId);
            }
            #endregion
            #region request
            TweetService.SearchForTweets(SearchQuery,
                result =>
                {
                    if (!result.HasError)
                    {
                        #region no search result
                        if (result.Statuses == null || result.Statuses.Count == 0)
                        {
                            App.HandleMessage(new ToastMessage
                            {
                                Message = LanguageHelper.GetString("Toast_Msg_NoSearchTweetResults", SearchQuery),
                            });
                        }
                        #endregion
                        #region add
                        else
                        {
#if !LOCAL
                            if (sinceId == result.Statuses[result.Statuses.Count - 1].Id)
                                result.Statuses.RemoveAt(result.Statuses.Count - 1);
#endif
                            for (int i = result.Statuses.Count - 1; i >= 0; i--)
                            {
                                var tweet = result.Statuses[i];
                                TweetList.Insert(0, new TweetViewModel(result.Statuses[i]));
                                if (TweetList.Count >= Const.DEFAULT_COUNT_VALUE)
                                    TweetList.RemoveAt(TweetList.Count - 1);
                            }
                        }
                        #endregion
                    }
                    base.Refreshed();
                }, parameters);
            #endregion
        }

        private void LoadAction()
        {
            if (TweetList.Count == 0)
            {
                base.Loaded();
                return;
            }
            #region parameter
            var parameters = TwitterHelper.GetDictionary();
            string maxId = TweetList[TweetList.Count - 1].Id;
            parameters.Add(Const.MAX_ID, maxId);
            #endregion
            #region request
            TweetService.SearchForTweets(SearchQuery,
                result =>
                {
                    #region handle error
                    if (result.HasError)
                    {
                        base.Loaded();
                        return;
                    }
                    #endregion
                    #region add
                    if (result.Statuses != null && result.Statuses.Count != 0)
                    {
#if !LOCAL
                        if (maxId == result.Statuses[0].Id)
                            result.Statuses.RemoveAt(0);
#endif
                        if (result.Statuses.Count != 0)
                        {
                            foreach (var tweet in result.Statuses)
                                TweetList.Add(new TweetViewModel(tweet));
                            base.Loaded();
                            return;
                        }
                    }
                    #endregion
                    #region no more results
                    App.HandleMessage(new ToastMessage
                    {
                        Message = LanguageHelper.GetString("Toast_Msg_NoMoreSearchTweetResults", SearchQuery)
                    });
                    base.Loaded();
                    #endregion
                }, parameters);
            #endregion
        }
        #endregion
    }
}

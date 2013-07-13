using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Search.VM
{
    public class SearchTweetViewModel : SearchViewModelBase
    {
        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public SearchTweetViewModel()
        {
            TweetList = new ObservableCollection<TweetViewModel>();
            RefreshHandler = this.RefreshAction;
        }

        #region override
        public override void Search()
        {
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
                                Message = LanguageHelper.GetString("Toast_Msg_NoSearchTweetResults"),
                            });
                        }
                        #endregion
                        #region add
                        else
                        {
                            for (int i = result.Statuses.Count - 1; i >= 0; i--)
                            {
#if !LOCAL
                                        if (sinceId == tweets[i].Id)
                                            continue;
#endif
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
        #endregion
    }
}

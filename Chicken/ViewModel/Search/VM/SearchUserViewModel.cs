using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Search.VM
{
    public class SearchUserViewModel : SearchViewModelBase
    {
        #region properties
        private int page = 1;
        #endregion

        public SearchUserViewModel()
        {
            UserList = new ObservableCollection<UserProfileViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
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
            #region request
            TweetService.SearchForUsers(SearchQuery,
                userProfileList =>
                {
                    if (!userProfileList.HasError)
                    {
                        #region no search result
                        if (userProfileList.Count == 0)
                        {
                            App.HandleMessage(new ToastMessage
                            {
                                Message = LanguageHelper.GetString("Toast_Msg_NoSearchUserResults", SearchQuery)
                            });
                        }
                        #endregion
                        #region add
                        else
                        {
                            UserList.Clear();
                            for (int i = userProfileList.Count - 1; i >= 0; i--)
                                UserList.Insert(0, new UserProfileViewModel(userProfileList[i]));
                        }
                        #endregion
                    }
                    base.Refreshed();
                });
            #endregion
        }

        private void LoadAction()
        {
            if (UserList.Count == 0)
            {
                base.Loaded();
                return;
            }
            #region parameters
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.PAGE, page);
            #endregion
            #region request
            TweetService.SearchForUsers(SearchQuery,
                userProfileList =>
                {
                    if (!userProfileList.HasError)
                    {
                        #region no search results
                        if (userProfileList.Count == 0)
                        {
                            App.HandleMessage(new ToastMessage
                            {
                                Message = LanguageHelper.GetString("Toast_Msg_NoMoreSearchUserResults", SearchQuery)
                            });
                        }
                        #endregion
                        else
                        #region add
                        {
                            foreach (var profile in userProfileList)
                                UserList.Add(new UserProfileViewModel(profile));
                            page += 1;
                        }
                        #endregion
                    }
                    base.Loaded();
                }, parameters);
            #endregion
        }
        #endregion
    }
}

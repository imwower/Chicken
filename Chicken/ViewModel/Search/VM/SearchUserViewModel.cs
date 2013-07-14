using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Base;
using System.Collections.ObjectModel;
using Chicken.Common;

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
                        if (userProfileList.Count == 0)
                        {
                            App.HandleMessage(new Model.ToastMessage
                            {
                                Message = LanguageHelper.GetString("Toast_Msg_NoSearchUserResults", SearchQuery)
                            });
                        }
                        else
                        {
                            UserList.Clear();
                            foreach (var profile in userProfileList)
                                UserList.Insert(0, new UserProfileViewModel(profile));
                        }
                    }
                    base.Refreshed();
                });
            #endregion
        }

        private void LoadAction()
        {
            #region parameters
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.PAGE, page);
            #endregion
        }
        #endregion
    }
}

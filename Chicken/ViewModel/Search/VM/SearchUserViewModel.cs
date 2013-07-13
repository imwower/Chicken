using Chicken.Service;
using Chicken.Service.Interface;

namespace Chicken.ViewModel.Search.VM
{
    public class SearchUserViewModel : SearchViewModelBase
    {
        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion
    }
}

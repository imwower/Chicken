using System.Collections.Generic;
using Chicken.Common;
using Chicken.Service;
using Chicken.Service.Interface;

namespace Chicken.ViewModel.Status
{
    public class StatusViewModelBase : ViewModelBase
    {
        #region properties
        private string statusId;
        public string StatusId
        {
            get
            {
                return statusId;
            }
            set
            {
                statusId = value;
                RaisePropertyChanged("StatusId");
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion
    }
}

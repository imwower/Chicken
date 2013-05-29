using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Profile.Base;
using System.Collections.Generic;
using Chicken.Common;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModelBase : ViewModelBase
    {
        #region properties
        private string userId;
        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                RaisePropertyChanged("UserId");
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        public override void Click(object parameter)
        {
            IsLoading = false;
            if (UserId == parameter.ToString())
            {
                NavigationServiceManager.ChangeSelectedIndex((int)Const.ProfilePageEnum.ProfileDetail);
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>(1);
                parameters.Add(Const.USER_ID, parameter);
                NavigationServiceManager.NavigateTo(Const.ProfilePage, parameters);
            }
        }
    }
}

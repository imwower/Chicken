using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Profile
{
    public class EditMyProfileViewModel : PivotItemViewModelBase
    {
        #region properties
        private UserProfileDetailViewModel myProfile;
        public UserProfileDetailViewModel MyProfile
        {
            get
            {
                return myProfile;
            }
            set
            {
                myProfile = value;
                RaisePropertyChanged("MyProfile");
            }
        }
        #endregion

        #region binding
        public ICommand SaveCommand
        {
            get
            {
                return new DelegateCommand(SaveAction);
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public EditMyProfileViewModel()
        {
            RefreshHandler = this.RefreshAction;
        }

        #region actions
        private void RefreshAction()
        {
            TweetService.GetMyProfileDetail(
                profile =>
                {
                    if (!profile.HasError)
                        MyProfile = new UserProfileDetailViewModel(profile);
                    base.Refreshed();
                });
        }

        private void SaveAction()
        {
            IsLoading = true;
            var parameters = TwitterHelper.GetDictionary();
            if (MyProfile.Name != App.AuthenticatedUser.Name)
                parameters.Add(Const.USER_NAME, MyProfile.Name);
            if (MyProfile.Location != App.AuthenticatedUser.Location)
                parameters.Add(Const.LOCATION, MyProfile.Location);
            if (MyProfile.Url != App.AuthenticatedUser.Url)
                parameters.Add(Const.URL, MyProfile.Url);
            parameters.Add(Const.DESCRIPTION, MyProfile.ExpandedDescription);
            TweetService.UpdateMyProfile(
                user =>
                {
                    IsLoading = false;
                    if (user.HasError)
                        return;
                    App.HandleMessage(new ToastMessage
                    {
                        Message = LanguageHelper.GetString("Toast_Msg_UpdateMyProfileSuccessfully"),
                        Complete =
                        () =>
                            NavigationServiceManager.NavigateTo(Const.ProfilePage, user)
                    });
                }, parameters);
        }
        #endregion
    }
}

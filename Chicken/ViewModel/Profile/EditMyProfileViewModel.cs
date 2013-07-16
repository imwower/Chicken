using System;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Profile
{
    public class EditMyProfileViewModel : PivotItemViewModelBase
    {
        #region event handler
        public Action BeforeSaveHandler;
        #endregion

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
            #region parameter
            var parameters = TwitterHelper.GetDictionary();
            if (!string.IsNullOrEmpty(MyProfile.Name))
                parameters.Add(Const.USER_NAME, MyProfile.Name);
            if (!string.IsNullOrEmpty(MyProfile.Location))
                parameters.Add(Const.LOCATION, MyProfile.Location);
            if (!string.IsNullOrEmpty(MyProfile.Url))
                parameters.Add(Const.URL, MyProfile.Url);
            if (!string.IsNullOrEmpty(MyProfile.ExpandedDescription))
                parameters.Add(Const.DESCRIPTION, MyProfile.ExpandedDescription);
            #endregion
            if (parameters.Count == 0)
                return;
            IsLoading = true;
            if (BeforeSaveHandler != null)
                BeforeSaveHandler();
            TweetService.UpdateMyProfile(
                user =>
                {
                    IsLoading = false;
                    if (user.HasError)
                        return;
                    App.HandleMessage(new ToastMessage
                    {
                        Message = LanguageHelper.GetString("Toast_Msg_UpdateMyProfileSuccessfully"),
                        Complete = () => NavigationServiceManager.NavigateTo(Const.ProfilePage, user)
                    });
                }, parameters);
        }
        #endregion
    }
}

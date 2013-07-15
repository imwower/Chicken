using System;
using System.Windows.Input;
using Chicken.Model;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Settings.VM
{
    public class AboutViewModel : SettingsViewModelBase
    {
        #region properties
        private bool isChicken;
        public bool IsChicken
        {
            get
            {
                return isChicken;
            }
            set
            {
                isChicken = value;
                RaisePropertyChanged("IsChicken");
            }
        }
        private UserProfileDetailViewModel userProfile;
        public UserProfileDetailViewModel UserProfile
        {
            get
            {
                return userProfile;
            }
            set
            {
                userProfile = value;
                RaisePropertyChanged("UserProfile");
            }
        }
        #endregion

        #region binding
        public ICommand FamousCommand
        {
            get
            {
                return new DelegateCommand(FamousAction);
            }
        }
        #endregion

        public AboutViewModel()
        {
            RefreshHandler = this.RefreshAction;
        }

        #region actions
        private void RefreshAction()
        {
            var profile = new UserProfileDetail
            {
                Id = "1519684519",
                ScreenName = "Chicken4WP",
                Name = "Chicken for Windows Phone",
                ProfileImage = "",
            };
            UserProfile = new UserProfileDetailViewModel(profile);
            IsChicken = true;
            base.Refreshed();
        }

        private void FamousAction()
        {
            if (App.Settings == null || App.Settings.APISettings == null)
                return;
            IsLoading = true;
            TweetService.GetFamous(
                profileList =>
                {
                    #region handle error
                    if (profileList.HasError || profileList.Count == 0)
                    {
                        IsLoading = false;
                        return;
                    }
                    #endregion
                    int index = new Random().Next(profileList.Count);
                    var user = profileList[index];
                    GetUserProfileDetail(user);
                });
        }
        #endregion

        #region private method
        private void GetUserProfileDetail(User user)
        {
            TweetService.GetUserProfileDetail(user,
                userProfileDetail =>
                {
                    IsLoading = false;
                    if (userProfileDetail.HasError)
                        return;
                    UserProfile = new UserProfileDetailViewModel(userProfileDetail);
                    IsChicken = false;
                });
        }
        #endregion
    }
}

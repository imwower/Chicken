using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Profile.Base;

namespace Chicken.ViewModel.Profile.VM
{
    public class ProfileDetailViewModel : ProfileViewModelBase
    {
        #region properties
        private UserProfileDetailViewModel userProfileViewModel;
        public UserProfileDetailViewModel UserProfileViewModel
        {
            get
            {
                return userProfileViewModel;
            }
            set
            {
                userProfileViewModel = value;
                RaisePropertyChanged("UserProfileViewModel");
            }
        }
        #endregion

        public ProfileDetailViewModel()
        {
            Header = "Profile";
        }

        public override void Refresh()
        {
            TweetService.GetUserProfileDetail<UserProfile>(UserId,
                obj =>
                {
                    this.UserProfileViewModel = new UserProfileDetailViewModel(obj);
                    base.Refresh();
                });
        }
    }
}

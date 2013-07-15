using System.Windows.Input;
using System.Xml.Linq;
using Chicken.Model;
using Chicken.Service;
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
        public ICommand UpdateDescriptionCommand
        {
            get
            {
                return new DelegateCommand(UpdateDescriptionAction);
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

        private void UpdateDescriptionAction()
        { }
        #endregion
    }
}

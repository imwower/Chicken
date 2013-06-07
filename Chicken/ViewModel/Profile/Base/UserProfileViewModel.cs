using Chicken.Model;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Profile.Base
{
    public class UserProfileViewModel : UserViewModel
    {
        protected UserProfile userProfile;

        public UserProfileViewModel(UserProfile userProfile) :
            base(userProfile)
        {
            this.userProfile = userProfile;
        }

        public string Description
        {
            get
            {
                if (!string.IsNullOrEmpty(userProfile.Description) && userProfile.Description.Length > 60)
                {
                    return userProfile.Description.Substring(0, 60) + "...";
                }
                return userProfile.Description;
            }
        }

        public string ProfileImageNormal
        {
            get
            {
                return userProfile.ProfileImage.Replace("_normal", "");
            }
        }

        public string UserProfileBannerImage
        {
            get
            {
                return userProfile.UserProfileBannerImage + "/web";
            }
        }
    }
}

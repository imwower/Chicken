using Chicken.Model;

namespace Chicken.ViewModel.Profile.Base
{
    public class UserProfileDetailViewModel : FriendProfileViewModel
    {
        public UserProfileDetailViewModel(UserProfile userProfile)
            : base(userProfile)
        { }

        public new string ProfileImage
        {
            get
            {
                return UserProfile.ProfileImage.Replace("_normal", "");
            }
        }

        public string UserProfileBannerImage
        {
            get
            {
                return UserProfile.UserProfileBannerImage + "/web";
            }
        }
    }
}

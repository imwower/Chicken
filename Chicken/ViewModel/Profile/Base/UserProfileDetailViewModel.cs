using Chicken.Model;

namespace Chicken.ViewModel.Profile.Base
{
    public class UserProfileDetailViewModel : UserProfileViewModel
    {
        public UserProfileDetailViewModel(UserProfile userProfile)
            : base(userProfile)
        { }

        public new string Description
        {
            get
            {
                return userProfile.Description;
            }
        }

        public bool IsFollowing
        {
            get
            {
                return userProfile.Following;
            }
        }
    }
}

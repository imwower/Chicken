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

        public string Location
        {
            get
            {
                return userProfile.Location;
            }
        }

        public string Url
        {
            get
            {
                return userProfile.Url;
            }
        }

        public string TweetsCount
        {
            get
            {
                return userProfile.TweetsCount;
            }
        }

        public string FollowingCount
        {
            get
            {
                return userProfile.FollowingCount;
            }
        }

        public string FollowersCount
        {
            get
            {
                return userProfile.FollowersCount;
            }
        }

        public string FavoritesCount
        {
            get
            {
                return userProfile.FavoritesCount;
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

using System.Collections.ObjectModel;
using Chicken.Model;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Profile.Base
{
    public class UserProfileViewModel : UserViewModel
    {
        public UserProfile UserProfile;

        public UserProfileViewModel(UserProfile userProfile) :
            base(userProfile)
        {
            this.UserProfile = userProfile;
        }

        public string Description
        {
            get
            {
                return UserProfile.Description;
            }
        }

        public string Location
        {
            get
            {
                return UserProfile.Location;
            }
        }

        public string Url
        {
            get
            {
                return UserProfile.Url;
            }
        }

        public string TweetsCount
        {
            get
            {
                return UserProfile.TweetsCount;
            }
        }

        public string FollowingCount
        {
            get
            {
                return UserProfile.FollowingCount;
            }
        }

        public string FollowersCount
        {
            get
            {
                return UserProfile.FollowersCount;
            }
        }

        public string FavoritesCount
        {
            get
            {
                return UserProfile.FavoritesCount;
            }
        }

        public string ProfileImageNormal
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

using System.Collections.ObjectModel;
using Chicken.Model;

namespace Chicken.ViewModel.Profile.Base
{
    public class FriendProfileViewModel
    {
        public UserProfile UserProfile;

        public FriendProfileViewModel(UserProfile userProfile)
        {
            this.UserProfile = userProfile;
        }

        public string Name
        {
            get
            {
                return UserProfile.Name;
            }
        }

        public string ScreenName
        {
            get
            {
                return UserProfile.ScreenName;
            }
        }

        public string Id
        {
            get
            {
                return UserProfile.Id;
            }
        }

        public string ProfileImage
        {
            get
            {
                return UserProfile.ProfileImage;
            }
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

        public string FavourtiesCount
        {
            get
            {
                return UserProfile.FavouritesCount;
            }
        }
    }
}

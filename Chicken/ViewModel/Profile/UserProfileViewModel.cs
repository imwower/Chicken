using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Chicken.Model;

namespace Chicken.ViewModel.Profile
{
    public class UserProfileViewModel
    {
        UserProfile userProfile;

        public UserProfileViewModel(UserProfile userProfile)
        {
            this.userProfile = userProfile;
        }

        public string Name
        {
            get
            {
                return userProfile.Name;
            }
        }

        public string ScreenName
        {
            get
            {
                return userProfile.ScreenName;
            }
        }

        public string Id
        {
            get
            {
                return userProfile.Id;
            }
        }

        public string ProfileImage
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

        public string Description
        {
            get
            {
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
    }
}

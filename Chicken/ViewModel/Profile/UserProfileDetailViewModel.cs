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

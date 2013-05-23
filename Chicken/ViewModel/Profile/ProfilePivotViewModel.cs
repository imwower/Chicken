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
using Chicken.Service;
using System.IO;
using Newtonsoft.Json;
using Chicken.Model;

namespace Chicken.ViewModel.Profile
{
    public class ProfilePivotViewModel : ProfileViewModelBase
    {
        UserProfileViewModel userProfileViewModel;
        public UserProfileViewModel UserProfileViewModel
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

        #region services
        public ITweetService TweetService = TweetServiceManger.TweetService;
        #endregion

        public ProfilePivotViewModel()
        {
            Header = "Profile";
        }

        public override void Refresh()
        {
            //var userProfile = TweetService.GetUserProfile(UserId);
            WebClient webClient = new WebClient();
            webClient.OpenReadAsync(new Uri("https://wxt2005.org/tapi/o/W699Q6//users/show.json?user_id=" + UserId, UriKind.Absolute));
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(
                (obj, e) =>
                {
                    using (StreamReader reader = new StreamReader(e.Result))
                    {
                        string output = reader.ReadToEnd();
                        var userProfile = JsonConvert.DeserializeObject<UserProfile>(output);
                        userProfile.ScreenName = "@" + userProfile.ScreenName;
                        this.UserProfileViewModel = new Profile.UserProfileViewModel(userProfile);
                    }
                });
        }
    }
}

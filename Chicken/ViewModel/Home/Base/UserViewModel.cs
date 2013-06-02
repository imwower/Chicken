using Chicken.Model;

namespace Chicken.ViewModel.Home.Base
{
    public class UserViewModel
    {
        private User user;

        public UserViewModel(User user)
        {
            this.user = user;
        }

        public string Name
        {
            get
            {
                return user.Name;
            }
        }

        public string ScreenName
        {
            get
            {
                return "@" + user.ScreenName;
            }
        }

        public string Id
        {
            get
            {
                return user.Id;
            }
        }

        public string ProfileImage
        {
            get
            {
                return user.ProfileImage;
            }
        }

        public string ProfileImageBigger
        {
            get
            {
                return user.ProfileImage.Replace("_normal", "_bigger");
            }
        }
    }
}

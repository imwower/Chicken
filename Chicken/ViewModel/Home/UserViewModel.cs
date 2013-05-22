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

namespace Chicken.ViewModel.Home
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

        public int Id
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
    }
}

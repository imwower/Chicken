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
using Chicken.Common;
using Chicken.Model;

namespace Chicken.ViewModel.Home
{
    public class DirectMessageViewModel
    {
        private DirectMessage directMessage;

        private UserViewModel user;
        public UserViewModel User
        {
            get
            {
                return user;
            }
        }

        public DirectMessageViewModel(DirectMessage directMessage)
        {
            this.directMessage = directMessage;
            this.user = new UserViewModel(directMessage.Sender);
        }

        public string Id
        {
            get
            {
                return directMessage.Id;
            }
        }

        public string Text
        {
            get
            {
                return directMessage.Text;
            }
        }

        public string CreatedDate
        {
            get
            {
                return directMessage.CreatedDate.ParseToDateTime();
            }
        }
    }
}

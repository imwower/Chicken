using Chicken.Common;
using Chicken.Model;

namespace Chicken.ViewModel.Home.Base
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
                return TwitterHelper.ParseToDateTime(directMessage.CreatedDate);
            }
        }
    }
}

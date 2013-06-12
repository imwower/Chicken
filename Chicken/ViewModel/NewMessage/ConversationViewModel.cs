using System.Collections.ObjectModel;
using Chicken.Model;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.NewMessage
{
    public class ConversationViewModel : NotificationObject
    {
        private User user;
        public User User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                RaisePropertyChanged("User");
            }
        }

        private ObservableCollection<DirectMessageViewModel> messages;
        public ObservableCollection<DirectMessageViewModel> Messages
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
                RaisePropertyChanged("Messages");
            }
        }

        public ConversationViewModel()
        {
            this.User = new User();
            this.Messages = new ObservableCollection<DirectMessageViewModel>();
        }
    }
}

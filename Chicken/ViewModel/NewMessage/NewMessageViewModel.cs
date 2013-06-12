using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Implementation;
using Chicken.Service.Interface;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.NewMessage
{
    public class NewMessageViewModel : PivotItemViewModelBase
    {
        #region properties
        private List<DirectMessageViewModel> messageList;
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
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public NewMessageViewModel()
        {
            Header = "Chat";
            User = new User();
            messageList = new List<DirectMessageViewModel>();
            Messages = new ObservableCollection<DirectMessageViewModel>();
            RefreshHandler = this.RefreshAction;
        }

        private void RefreshAction()
        {
            if (user == null || user.Id == null)
            {
                return;
            }
            TweetService.GetDirectMessages<DirectMessageList<DirectMessage>>(
               messages =>
               {
                   if (messages != null && messages.Count != 0)
                   {
                       foreach (var message in messages)
                       {
                           if (message.User.Id == user.Id)
                           {
                               messageList.Add(new DirectMessageViewModel(message));
                           }
                       }
                   };

                   GetDirectMessagesSentByMe();
               });
        }

        private void GetDirectMessagesSentByMe()
        {
            TweetService.GetDirectMessagesSentByMe<DirectMessageList<DirectMessage>>(
                messages =>
                {
                    if (messages != null && messages.Count != 0)
                    {
                        foreach (var message in messages)
                        {
                            if (message.Receiver.Id == user.Id)
                            {
                                message.IsSentByMe = true;
                                message.User = message.Receiver;
                                messageList.Add(new DirectMessageViewModel(message));
                            }
                        }
                    }

                    messageList = messageList.OrderBy(m => m.CreatedDate).ToList();
                    Messages.Clear();

                    foreach (var message in messageList)
                    {
                        Messages.Add(message);
                    }
                });
            base.Refreshed();
        }
    }
}

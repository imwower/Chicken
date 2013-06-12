using System.Collections.Generic;
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
        private ConversationViewModel conversationViewModel;
        public ConversationViewModel ConversationViewModel
        {
            get
            {
                return conversationViewModel;
            }
            set
            {
                conversationViewModel = value;
                RaisePropertyChanged("ConversationViewModel");
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public NewMessageViewModel()
        {
            Header = "Chat";
            ConversationViewModel = new ConversationViewModel();
            ConversationViewModel.User.Id = "514378421";
            messageList = new List<DirectMessageViewModel>();
            RefreshHandler = this.RefreshAction;
        }

        private void RefreshAction()
        {
            TweetService.GetDirectMessages<DirectMessageList<DirectMessage>>(
               messages =>
               {
                   if (messages != null && messages.Count != 0)
                   {
                       foreach (var message in messages)
                       {
                           if (message.User.Id == conversationViewModel.User.Id)
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
                            if (message.Receiver.Id == ConversationViewModel.User.Id)
                            {
                                message.User = message.Receiver;
                                messageList.Add(new DirectMessageViewModel(message) { IsSentByMe = true });
                            }
                        }
                    }

                    ConversationViewModel.Messages.Clear();
                    messageList = messageList.OrderBy(m => m.CreatedDate).ToList();
                    foreach (var message in messageList)
                    {
                        ConversationViewModel.Messages.Add(message);
                    }
                });
        }
    }
}

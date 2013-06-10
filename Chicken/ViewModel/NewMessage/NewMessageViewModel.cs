using System.Collections.ObjectModel;
using Chicken.Common;
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
        private ObservableCollection<DirectMessageViewModel> conversationList;
        public ObservableCollection<DirectMessageViewModel> ConversationList
        {
            get
            {
                return conversationList;
            }
            set
            {
                conversationList = value;
                RaisePropertyChanged("ConversationList");
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public NewMessageViewModel()
        {
            Header = "Chat";
            ConversationList = new ObservableCollection<DirectMessageViewModel>();
            RefreshHandler = this.RefreshAction;
        }

        private void RefreshAction()
        {
            string sinceId = string.Empty;
            var parameters = TwitterHelper.GetDictionary();
            if (ConversationList.Count != 0)
            {
                sinceId = ConversationList[0].Id;
                parameters.Add(Const.SINCE_ID, sinceId);
            }
            TweetService.GetDirectMessages<DirectMessageList<DirectMessage>>(
               messages =>
               {
                   if (messages != null && messages.Count != 0)
                   {
#if !DEBUG
                       if (string.Compare(sinceId, messages[0].Id) == -1)
                       {
                           TweetList.Clear();
                       }
#endif
                       for (int i = messages.Count - 1; i >= 0; i--)
                       {
#if !DEBUG
                           if (sinceId != messages[i].Id)
#endif
                           {
                               ConversationList.Insert(0, new DirectMessageViewModel(messages[i]));
                           }
                       }
                   }
                   base.Refreshed();
               }, parameters);
        }
    }
}

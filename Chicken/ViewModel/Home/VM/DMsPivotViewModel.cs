using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Home.VM
{
    public class DMsPivotViewModel : HomeViewModelBase
    {
        private ObservableCollection<DirectMessageViewModel> dmList;
        public ObservableCollection<DirectMessageViewModel> DMList
        {
            get
            {
                return dmList;
            }
            set
            {
                dmList = value;
                RaisePropertyChanged("DMList");
            }
        }

        public DMsPivotViewModel()
        {
            Header = "Messages";
            DMList = new ObservableCollection<DirectMessageViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
        }

        private void RefreshAction()
        {
            string sinceId = string.Empty;
            var parameters = TwitterHelper.GetDictionary();
            if (dmList.Count != 0)
            {
                sinceId = dmList[0].Id;
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
                           DMList.Clear();
                       }
#endif
                       for (int i = messages.Count - 1; i >= 0; i--)
                       {
#if !DEBUG
                           if (sinceId != messages[i].Id)
#endif
                           {
                               DMList.Insert(0, new DirectMessageViewModel(messages[i]));
                           }
                       }
                   }
               }, parameters);
        }

        private void LoadAction()
        {
            if (dmList.Count == 0)
            {
                return;
            }
            else
            {
                string maxId = dmList[dmList.Count - 1].Id;
                var parameters = TwitterHelper.GetDictionary();
                parameters.Add(Const.MAX_ID, maxId);
                TweetService.GetDirectMessages<DirectMessageList<DirectMessage>>(
                    messages =>
                    {
                        foreach (var message in messages)
                        {
#if !DEBUG
                            if (maxId != message.Id)
#endif
                            {
                                DMList.Add(new DirectMessageViewModel(message));
                            }
                        }
                    }, parameters);
            }
        }
    }
}

using System.Collections.Generic;
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
        }

        public override void Refresh()
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;
            string sinceId = string.Empty;
            var parameters = TwitterHelper.GetDictionary();
            if (dmList.Count != 0)
            {
                sinceId = dmList[0].Id;
                parameters.Add(Const.SINCE_ID, sinceId);
            }
            TweetService.GetDirectMessages<List<DirectMessage>>(
               messages =>
               {
                   if (messages != null && messages.Count != 0)
                   {
                       messages.Reverse();
                       if (string.Compare(sinceId, messages[0].Id) == -1)
                       {
                           DMList.Clear();
                       }
                       foreach (var message in messages)
                       {
                           if (sinceId != message.Id)
                           {
                               DMList.Insert(0, new DirectMessageViewModel(message));
                           }
                       }
                   }
                   base.Refreshed();
               }, parameters);
        }

        public override void Load()
        {
            if (IsLoading)
            {
                return;
            }
            if (dmList.Count == 0)
            {
                base.Load();
                return;
            }
            else
            {
                IsLoading = true;
                string maxId = dmList[dmList.Count - 1].Id;
                var parameters = TwitterHelper.GetDictionary();
                parameters.Add(Const.MAX_ID, maxId);
                TweetService.GetDirectMessages<List<DirectMessage>>(
                    messages =>
                    {
                        foreach (var message in messages)
                        {
                            if (maxId != message.Id)
                            {
                                DMList.Add(new DirectMessageViewModel(message));
                            }
                        }
                        base.Load();
                    }, parameters);
            }
        }
    }
}

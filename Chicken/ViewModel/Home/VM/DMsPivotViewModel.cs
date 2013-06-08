using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Home.VM
{
    public class DMsPivotViewModel : HomeViewModelBase
    {
        public DMsPivotViewModel()
        {
            Header = "Messages";
            TweetList = new ObservableCollection<TweetViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
        }

        private void RefreshAction()
        {
            string sinceId = string.Empty;
            var parameters = TwitterHelper.GetDictionary();
            if (TweetList.Count != 0)
            {
                sinceId = TweetList[0].Id;
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
                               TweetList.Insert(0, new DirectMessageViewModel(messages[i]));
                           }
                       }
                   }
                   base.Refreshed();
               }, parameters);
        }

        private void LoadAction()
        {
            if (TweetList.Count == 0)
            {
                base.Loaded();
                return;
            }
            else
            {
                string maxId = TweetList[TweetList.Count - 1].Id;
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
                                TweetList.Add(new DirectMessageViewModel(message));
                            }
                        }
                        base.Loaded();
                    }, parameters);
            }
        }
    }
}

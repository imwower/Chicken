using System.Linq;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.ViewModel.Home.Base;
using System.Collections.Generic;
using Chicken.Service;

namespace Chicken.ViewModel.Home.VM
{
    public class DMsPivotViewModel : HomeViewModelBase
    {
        private LatestMessagesModel latestMessages;
        private List<DirectMessage> list;

        public DMsPivotViewModel()
        {
            Header = "Messages";
            list = new List<DirectMessage>();
            TweetList = new ObservableCollection<TweetViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
            ItemClickHandler = this.ItemClickAction;
        }

        private void RefreshAction()
        {
            #region init from file
            var file = IsolatedStorageService.GetObject<LatestMessagesModel>(Const.PageNameEnum.MainPage, Const.LATEST_MESSAGES_FILENAME);
            if (file == null)
            {
                latestMessages = new LatestMessagesModel();
            }
            else
            {
                latestMessages = file;
            }
            #endregion

            #region get received messages
            TweetService.GetDirectMessages<DirectMessageList<DirectMessage>>(
                messages =>
                {
                    if (messages != null && messages.Count != 0)
                    {
                        latestMessages.SinceId = messages.First().Id;
                        latestMessages.MaxId = messages.Last().Id;

                        foreach (var message in messages)
                        {
                            list.Add(message);
                        }
                        GetDirectMessagesSentByMe();
                    }
                });
            #endregion
        }

        private void GetDirectMessagesSentByMe()
        {
            TweetService.GetDirectMessagesSentByMe<DirectMessageList<DirectMessage>>(
                messages =>
                {
                    #region get messsages
                    if (messages != null && messages.Count != 0)
                    {
                        latestMessages.SinceIdByMe = messages.First().Id;
                        latestMessages.MaxIdByMe = messages.Last().Id;

                        foreach (var message in messages)
                        {
                            message.IsSentByMe = true;
                            message.User = message.Receiver;
                            list.Add(message);
                        }
                    }
                    #endregion

                    var dict = new Dictionary<string, Conversation>();
                    latestMessages.Messages.Clear();
                    var group = list.OrderByDescending(m => m.Id).GroupBy(u => u.User.Id);

                    foreach (var msgs in group)
                    {
                        foreach (var msg in msgs)
                        {
                            if (!msg.IsOld)
                            {
                                if (!dict.ContainsKey(msgs.Key))
                                {
                                    dict.Add(msgs.Key, new Conversation());
                                }
                                dict[msgs.Key].Messages.Add(msg);
                            }
                        }

                        IsolatedStorageService.AddMessages(dict[msgs.Key]);

                        #region store current msgs
                        var first = msgs.First();
                        first.IsOld = true;
                        TweetList.Add(new TweetViewModel(first));
                        latestMessages.Messages.Add(first);
                        #endregion
                    }

                    IsolatedStorageService.CreateObject(Const.PageNameEnum.MainPage, Const.LATEST_MESSAGES_FILENAME, latestMessages);
                    base.Refreshed();
                });
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

        private void ItemClickAction(object user)
        {
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.NewMessagePage, user);
        }
    }
}

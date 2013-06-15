using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Home.VM
{
    public class DMsPivotViewModel : HomeViewModelBase
    {
        #region properties
        private LatestMessagesModel latestMessages;
        private List<DirectMessage> list;
        private bool hasMoreMsgs = true;
        private bool hasMoreMsgsByMe = true;
        private Dictionary<string, Conversation> dict;
        #endregion

        public DMsPivotViewModel()
        {
            Header = "Messages";
            list = new List<DirectMessage>();
            dict = new Dictionary<string, Conversation>();
            latestMessages = new LatestMessagesModel();
            TweetList = new ObservableCollection<TweetViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
            ItemClickHandler = this.ItemClickAction;
        }

        private void RefreshAction()
        {
            #region init
            var file = IsolatedStorageService.GetLatestMessages();
            if (file != null)
            {
                latestMessages = file;
            }
            #endregion
            GetReceivedMessages();
        }

        private void GetReceivedMessages(bool isLoadAction = false)
        {
            #region parameters
            var parameters = TwitterHelper.GetDictionary();
            if (!isLoadAction)
            {
                if (!string.IsNullOrEmpty(latestMessages.SinceId))
                {
                    parameters.Add(Const.SINCE_ID, latestMessages.SinceId);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(latestMessages.MaxId))
                {
                    parameters.Add(Const.MAX_ID, latestMessages.MaxId);
                }
            }
            #endregion

            TweetService.GetDirectMessages<DirectMessageList<DirectMessage>>(
                messages =>
                {
                    if (messages != null && messages.Count != 0)
                    {
                        foreach (var message in messages)
                        {
                            if (message.Id != latestMessages.MaxId)
                            {
                                list.Add(message);
                            }
                        }
                        if (!isLoadAction)
                        {
                            latestMessages.SinceId = messages.First().Id;
                        }
                        latestMessages.MaxId = messages.Last().Id;
                    }
                    else
                    {
                        if (isLoadAction)
                            hasMoreMsgs = false;
                    }

                    GetDirectMessagesSentByMe(isLoadAction);
                }, parameters);
        }

        private void GetDirectMessagesSentByMe(bool isLoadAction = false)
        {
            #region parameters
            var parameters = TwitterHelper.GetDictionary();
            if (!isLoadAction)
            {
                if (!string.IsNullOrEmpty(latestMessages.SinceIdByMe))
                {
                    parameters.Add(Const.SINCE_ID, latestMessages.SinceIdByMe);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(latestMessages.MaxIdByMe))
                {
                    parameters.Add(Const.MAX_ID, latestMessages.MaxIdByMe);
                }
            }
            #endregion

            TweetService.GetDirectMessagesSentByMe<DirectMessageList<DirectMessage>>(
                messages =>
                {
                    #region get messsages
                    if (messages != null && messages.Count != 0)
                    {
                        foreach (var message in messages)
                        {
                            if (message.Id != latestMessages.MaxIdByMe)
                            {
                                message.IsSentByMe = true;
                                message.User = message.Receiver;
                                list.Add(message);
                            }
                        }
                        if (!isLoadAction)
                        {
                            latestMessages.SinceIdByMe = messages.First().Id;
                        }
                        latestMessages.MaxIdByMe = messages.Last().Id;
                    }
                    else
                    {
                        if (isLoadAction)
                            hasMoreMsgsByMe = false;
                    }
                    #endregion

                    #region group
                    var groups = list.OrderByDescending(m => m.Id).GroupBy(u => u.User.Id);
                    foreach (var msgs in groups)
                    {
                        foreach (var msg in msgs)
                        {
                            if (!dict.ContainsKey(msgs.Key))
                            {
                                dict.Add(msgs.Key, new Conversation());
                            }
                            dict[msgs.Key].Messages.Add(msg);
                        }
                        IsolatedStorageService.AddMessages(dict[msgs.Key]);

                        #region store latest msgs
                        if (!isLoadAction)
                        {
                            var first = msgs.First();
                            latestMessages.Messages[first.User.Id] = first;
                        }
                        #endregion
                    }
                    #endregion
                    #region finish
                    if (!isLoadAction)
                    {
                        var latestmsgs = latestMessages.Messages.Values.OrderBy(m => m.Id);
                        var latestId = TweetList.Count == 0 ? string.Empty : TweetList.First().Id;
                        foreach (var msg in latestmsgs)
                        {
                            if (string.Compare(msg.Id, latestId) > 0)
                                TweetList.Insert(0, new TweetViewModel(msg));
                        }
                    }
                    else
                    {
                        var latestmsgs = latestMessages.Messages.Values.OrderByDescending(m => m.Id);
                        var oldestId = TweetList.Count == 0 ? string.Empty : TweetList.Last().Id;
                        foreach (var msg in latestmsgs)
                        {
                            if (string.Compare(msg.Id, oldestId) < 0)
                                TweetList.Add(new TweetViewModel(msg));
                        }
                    }
                    base.Refreshed();
                    list.Clear();
                    dict.Clear();
                    IsolatedStorageService.AddLatestMessages(latestMessages);
                    #endregion
                }, parameters);
        }

        private void LoadAction()
        {
            if (hasMoreMsgs && hasMoreMsgsByMe)
            {
                GetReceivedMessages(true);
            }
            else
            {
                LoadHandler = null;
                base.Loaded();
            }
        }

        private void ItemClickAction(object user)
        {
            var newMessage = new NewMessageModel
            {
                User = user as User,
            };
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.NewMessagePage, newMessage);
        }
    }
}

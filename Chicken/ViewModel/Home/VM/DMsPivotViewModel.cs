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

        #region action
        private void RefreshAction()
        {
            #region init
            var file = IsolatedStorageService.GetLatestMessages();
            if (file != null)
            {
                latestMessages = file;
            }
            #endregion
            RefreshReceivedMessages();
        }

        private void LoadAction()
        {
            if (hasMoreMsgs && hasMoreMsgsByMe)
            {
                LoadReceivedMessages();
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
        #endregion

        #region private
        private void RefreshReceivedMessages()
        {
            #region parameters
            var parameters = TwitterHelper.GetDictionary();
            if (!string.IsNullOrEmpty(latestMessages.SinceId))
            {
                parameters.Add(Const.SINCE_ID, latestMessages.SinceId);
            }
            #endregion
            TweetService.GetDirectMessages<DirectMessageList<DirectMessage>>(
                messages =>
                {
                    if (messages != null && messages.Count != 0)
                    {
                        foreach (var message in messages)
                        {
                            list.Add(message);
                        }
                        latestMessages.SinceId = messages.First().Id;
                        latestMessages.MaxId = messages.Last().Id;
                    }
                    RefreshDirectMessagesSentByMe();
                }, parameters);
        }

        private void RefreshDirectMessagesSentByMe()
        {
            #region parameters
            var parameters = TwitterHelper.GetDictionary();
            if (!string.IsNullOrEmpty(latestMessages.SinceIdByMe))
            {
                parameters.Add(Const.SINCE_ID, latestMessages.SinceIdByMe);
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
                            message.IsSentByMe = true;
                            message.User = message.Receiver;
                            list.Add(message);
                        }
                        latestMessages.SinceIdByMe = messages.First().Id;
                        latestMessages.MaxIdByMe = messages.Last().Id;
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
                        var first = msgs.First();
                        latestMessages.Messages[first.User.Id] = first;
                        #endregion
                    }
                    #endregion
                    IsolatedStorageService.AddLatestMessages(latestMessages);
                    FinishRefreshing();
                }, parameters);
        }

        private void FinishRefreshing()
        {
            TweetList.Clear();
            var latestmsgs = latestMessages.Messages.Values.OrderBy(m => m.Id);
            foreach (var msg in latestmsgs)
            {
                TweetList.Insert(0, new TweetViewModel(msg));
            }
            ScrollTo = ScrollTo.Top;
            base.Refreshed();
            list.Clear();
            dict.Clear();
        }

        private void LoadReceivedMessages()
        {
            #region parameters
            var parameters = TwitterHelper.GetDictionary();
            if (!string.IsNullOrEmpty(latestMessages.MaxId))
            {
                parameters.Add(Const.MAX_ID, latestMessages.MaxId);
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
                                list.Add(message);
                        }
                        latestMessages.MaxId = messages.Last().Id;
                    }
                    else
                    {
                        hasMoreMsgs = false;
                    }
                    LoadDirectMessagesSentByMe();
                }, parameters);
        }

        private void LoadDirectMessagesSentByMe()
        {
            #region parameters
            var parameters = TwitterHelper.GetDictionary();
            if (!string.IsNullOrEmpty(latestMessages.MaxIdByMe))
            {
                parameters.Add(Const.MAX_ID, latestMessages.MaxIdByMe);
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
                        latestMessages.MaxIdByMe = messages.Last().Id;
                    }
                    else
                    {
                        hasMoreMsgsByMe = false;
                    }
                    #endregion
                    #region group
                    var groups = list.GroupBy(u => u.User.Id);
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
                    }
                    #endregion
                    IsolatedStorageService.AddLatestMessages(latestMessages);
                    FinishLoading();
                }, parameters);
        }

        private void FinishLoading()
        {
            TweetList.Clear();
            var latestmsgs = latestMessages.Messages.Values.OrderByDescending(m => m.Id);
            foreach (var msg in latestmsgs)
            {
                TweetList.Add(new TweetViewModel(msg));
            }
            ScrollTo = ScrollTo.Bottom;
            base.Refreshed();
            list.Clear();
            dict.Clear();
        }
        #endregion
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Implementation;
using Chicken.ViewModel.Base;
using Chicken.ViewModel.NewTweet;

namespace Chicken.ViewModel.NewMessage
{
    public class NewMessageViewModel : NewTweetViewModel
    {
        #region properties
        private LatestMessagesModel latestMessages;
        private List<DirectMessage> list;
        private bool hasMoreMsgs = true;
        private bool hasMoreMsgsByMe = true;
        private Dictionary<string, Conversation> dict;
        public ObservableCollection<DirectMessageViewModel> Messages { get; set; }
        private NewMessageModel newMessage;
        public bool IsNew
        {
            get
            {
                return newMessage.IsNew;
            }
            set
            {
                newMessage.IsNew = value;
                RaisePropertyChanged("IsNew");
            }
        }
        public User User
        {
            get
            {
                return newMessage.User;
            }
            set
            {
                newMessage.User = value;
                RaisePropertyChanged("User");
            }
        }
        public override string Text
        {
            get
            {
                return newMessage.Text;
            }
            set
            {
                newMessage.Text = value;
                RaisePropertyChanged("Text");
            }
        }
        private bool hasError;
        public bool HasError
        {
            get
            {
                return hasError;
            }
            set
            {
                hasError = value;
                RaisePropertyChanged("HasError");
            }
        }
        #endregion

        public NewMessageViewModel()
        {
            Title = LanguageHelper.GetString("NewMessagePage_Header");
            list = new List<DirectMessage>();
            dict = new Dictionary<string, Conversation>();
            Messages = new ObservableCollection<DirectMessageViewModel>();
            newMessage = new NewMessageModel { User = new User() };
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
            ClickHandler = this.ClickAction;
        }

        public void ValidateUser()
        {
            if (!string.IsNullOrEmpty(User.ScreenName))
            {
                User.ScreenName = User.ScreenName.Replace("@", "").Replace(" ", "");
                if (string.IsNullOrEmpty(User.ScreenName))
                    HasError = true;
                else
                    hasError = false;
            }
            else
                HasError = true;
            if (HasError)
            {
                Title = LanguageHelper.GetString("NewMessagePage_Header_NotAValidUserName");
                App.HandleMessage(new ToastMessage
                {
                    Message = Title
                });
                return;
            }
            CheckIfUserExists();
        }

        #region actions
        private void RefreshAction()
        {
            var file = IsolatedStorageService.GetObject<NewMessageModel>(Const.NewMessagePage);
            if (file == null)
            {
                IsNew = true;
                base.Refreshed();
                return;
            }
            newMessage = file;
            User = newMessage.User;
            IsNew = false;
            latestMessages = IsolatedStorageService.GetLatestMessages();
            if (latestMessages == null)
                latestMessages = new LatestMessagesModel();
            Title = LanguageHelper.GetString("NewMessagePage_Header_ChatWithUser", User.DisplayName);
            RefreshReceivedMessages();
        }

        private void LoadAction()
        {
            if (hasMoreMsgs || hasMoreMsgsByMe)
            {
                LoadReceivedMessages();
            }
            else
            {
                LoadHandler = null;
                base.Loaded();
            }
        }

        private void ClickAction(object parameter)
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(Const.ProfilePage, parameter);
        }

        protected override void SendAction()
        {
            #region validate user name and text
            if (IsLoading
                || HasError
                || string.IsNullOrEmpty(newMessage.User.ScreenName)
                || string.IsNullOrEmpty(newMessage.Text))
                return;
            #endregion
            #region post new message
            IsLoading = true;
            if (BeforeSendHandler != null)
                BeforeSendHandler();
            TweetService.PostNewMessage(newMessage,
                message =>
                {
                    #region handle error
                    if (message.HasError)
                    {
                        IsLoading = false;
                        return;
                    }
                    #endregion
                    newMessage.User = message.Receiver;
                    IsNew = false;
                    Text = string.Empty;
                    IsolatedStorageService.CreateObject(Const.NewMessagePage, newMessage);
                    App.HandleMessage(new ToastMessage
                    {
                        Message = LanguageHelper.GetString("Toast_Msg_MessageSentSuccessfully"),
                    });
                    RefreshAction();
                });
            #endregion
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
            TweetService.GetDirectMessages(
                messages =>
                {
                    if (!messages.HasError && messages.Count != 0)
                    {
                        foreach (var message in messages)
                            list.Add(message);
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
            TweetService.GetDirectMessagesSentByMe(
                messages =>
                {
                    #region get messsages
                    if (!messages.HasError && messages.Count != 0)
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
                                dict.Add(msgs.Key, new Conversation());
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
            var conversation = IsolatedStorageService.GetMessages(User.Id);
            if (conversation != null)
            {
                var latestId = Messages.Count == 0 ? string.Empty : Messages.Last().Id;
                var msgs = conversation.Messages.OrderBy(m => m.Id);
                foreach (var msg in msgs)
                {
                    if (string.Compare(msg.Id, latestId) > 0)
                    {
                        msg.User = newMessage.User;
                        Messages.Add(new DirectMessageViewModel(msg));
                    }
                }
            }
            ScrollTo = ScrollTo.Bottom;
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
            TweetService.GetDirectMessages(
                messages =>
                {
                    if (!messages.HasError)
                    {
                        if (messages.Count != 0)
                        {
                            foreach (var message in messages)
                                if (message.Id != latestMessages.MaxId)
                                    list.Add(message);
                            latestMessages.MaxId = messages.Last().Id;
                        }
                        else
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
            TweetService.GetDirectMessagesSentByMe(
                messages =>
                {
                    #region get messsages
                    if (!messages.HasError)
                    {
                        if (messages.Count != 0)
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
                                dict.Add(msgs.Key, new Conversation());
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
            var conversation = IsolatedStorageService.GetMessages(User.Id);
            if (conversation != null)
            {
                var oldestId = Messages.Count == 0 ? string.Empty : Messages.First().Id;
                var msgs = conversation.Messages.OrderByDescending(m => m.Id);
                foreach (var msg in msgs)
                {
                    if (string.Compare(msg.Id, oldestId) < 0)
                    {
                        msg.User = newMessage.User;
                        Messages.Insert(0, new DirectMessageViewModel(msg));
                    }
                }
            }
            ScrollTo = ScrollTo.Top;
            base.Loaded();
            list.Clear();
            dict.Clear();
        }

        private void CheckIfUserExists()
        {
            TweetService.GetUser(User.ScreenName,
                user =>
                {
                    if (user.HasError)
                    {
                        HasError = true;
                        return;
                    }
                    User = user;
                    ValidateFriendship();
                });
        }

        private void ValidateFriendship()
        {
            TweetService.GetFriendshipConnections(User.Id,
                friendships =>
                {
                    if (!friendships.HasError && friendships.Count != 0)
                    {
                        List<string> connections = friendships[0].Connections;
                        if (!connections.Contains(Const.FOLLOWED_BY))
                        {
                            HasError = true;
                            Title = LanguageHelper.GetString("NewMessagePage_Header_UserNotFollowYou", User.DisplayName);
                            App.HandleMessage(new ToastMessage
                            {
                                Message = Title
                            });
                            return;
                        }
                        HasError = false;
                        IsolatedStorageService.CreateObject(Const.NewMessagePage, newMessage);
                        RefreshAction();
                    }
                });
        }
        #endregion
    }
}

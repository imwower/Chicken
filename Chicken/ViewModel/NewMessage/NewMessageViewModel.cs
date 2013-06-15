using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
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
        #region event handler
        public delegate void AddEmotionEventHandler();
        public AddEmotionEventHandler AddEmotionHandler;
        public AddEmotionEventHandler KeyboardHandler;
        #endregion

        #region properties
        private LatestMessagesModel latestMessages;
        private List<DirectMessage> list;
        private bool hasMoreMsgs = true;
        private bool hasMoreMsgsByMe = true;
        private Dictionary<string, Conversation> dict;

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
        private AppBarState state;
        public AppBarState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                RaisePropertyChanged("State");
            }
        }

        public NewMessageModel NewMessage { get; set; }
        public string Text
        {
            get
            {
                return NewMessage.Text;
            }
            set
            {
                NewMessage.Text = value;
                RaisePropertyChanged("Text");
            }
        }
        public bool IsNew
        {
            get
            {
                return NewMessage.IsNew;
            }
            set
            {
                NewMessage.IsNew = value;
                RaisePropertyChanged("IsNew");
            }
        }
        public User User
        {
            get
            {
                return NewMessage.User;
            }
            set
            {
                NewMessage.User = value;
                RaisePropertyChanged("User");
            }
        }
        #endregion

        #region binding
        public ICommand SendCommand
        {
            get
            {
                return new DelegateCommand(SendAction);
            }
        }

        //public ICommand MentionCommand
        //{
        //    get
        //    {
        //        return new DelegateCommand(MentionAction);
        //    }
        //}

        public ICommand AddEmotionCommand
        {
            get
            {
                return new DelegateCommand(AddEmotionAction);
            }
        }

        public ICommand KeyboardCommand
        {
            get
            {
                return new DelegateCommand(KeyboardAction);
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public NewMessageViewModel()
        {
            Header = "Chat";
            list = new List<DirectMessage>();
            dict = new Dictionary<string, Conversation>();
            latestMessages = new LatestMessagesModel();
            Messages = new ObservableCollection<DirectMessageViewModel>();
            NewMessage = new NewMessageModel();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
            ClickHandler = this.ClickAction;
        }

        public void ValidateFriendship()
        {
            ValidateUserName();
            if (HasError)
            {
                return;
            }
            TweetService.GetUser<User>(User.DisplayName,
                user =>
                {
                    #region user existes or not:
                    List<ErrorMessage> errors = user.Errors;
                    if (errors != null && errors.Count != 0)
                    {
                        //user not exist:
                        HasError = true;
                        Header = User.ScreenName + " does not exist";
                        ToastMessageHandler(new ToastMessage
                        {
                            Message = Header
                        });
                        return;
                    }
                    #endregion
                    #region user followed you or not
                    TweetService.GetFriendships<Friendships<Friendship>>(User.DisplayName,
                        friendships =>
                        {
                            Friendship friendship = friendships[0];
                            List<string> connections = friendship.Connections;
                            if (!connections.Contains(Const.FOLLOWED_BY))
                            {
                                HasError = true;
                                Header = User.ScreenName + " did not follow you";
                                ToastMessageHandler(new ToastMessage
                                {
                                    Message = Header
                                });
                                return;
                            }
                            HasError = false;
                            IsNew = false;
                            User.Id = friendship.Id;
                            RefreshAction();
                        });
                    #endregion
                });
        }

        #region actions
        private void RefreshAction()
        {
            if (IsNew)
            {
                User = new User();
                base.Refreshed();
                return;
            }
            Header = User.ScreenName;
            #region init from file
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

        private void ClickAction(object parameter)
        {
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.ProfilePage, parameter);
        }

        private void SendAction()
        {
            #region validate user name and text
            if (IsLoading || HasError ||
                string.IsNullOrEmpty(NewMessage.User.DisplayName)
                || string.IsNullOrEmpty(NewMessage.Text))
            {
                return;
            }
            #endregion
            #region post new message
            TweetService.PostNewMessage<DirectMessage>(NewMessage.User.DisplayName, NewMessage.Text,
                message =>
                {
                    List<ErrorMessage> errors = message.Errors;
                    if (errors != null && errors.Count != 0)
                    {
                        ToastMessageHandler(new ToastMessage
                        {
                            Message = errors[0].Message
                        });
                    }
                    else
                    {
                        this.User = message.Receiver;
                        Text = string.Empty;
                        IsNew = false;
                        RefreshAction();
                        //
                        ToastMessageHandler(new ToastMessage
                        {
                            Message = "message sent successfully"
                        });
                    }
                });
            #endregion
        }

        private void AddEmotionAction()
        {
            if (AddEmotionHandler != null)
            {
                State = AppBarState.AddEmotion;
                AddEmotionHandler();
            }
        }

        private void KeyboardAction()
        {
            if (KeyboardHandler != null)
            {
                State = AppBarState.Default;
                KeyboardHandler();
            }
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
            //else
            //{
            //    if (!string.IsNullOrEmpty(latestMessages.MaxId))
            //    {
            //        parameters.Add(Const.MAX_ID, latestMessages.MaxId);
            //    }
            //}
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
                        Messages.Add(new DirectMessageViewModel(msg));
                }
                ScrollTo = ScrollTo.Top;
            }
            base.Refreshed();
            list.Clear();
            dict.Clear();
            IsolatedStorageService.AddLatestMessages(latestMessages);
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
                    FinishLoading();
                }, parameters);
        }

        private void FinishLoading()
        {
            var conversation = IsolatedStorageService.GetMessages(User.Id);
            if (conversation != null)
            {
                var oldestId = Messages.Count == 0 ? string.Empty : Messages.First().Id;
                var msgs = conversation.Messages.OrderBy(m => m.Id);
                foreach (var msg in msgs)
                {
                    if (string.Compare(msg.Id, oldestId) < 0)
                        Messages.Insert(0, new DirectMessageViewModel(msg));
                }
                ScrollTo = ScrollTo.Bottom;
            }
            base.Loaded();
            list.Clear();
            dict.Clear();
            IsolatedStorageService.AddLatestMessages(latestMessages);
        }

        private void ValidateUserName()
        {
            if (!string.IsNullOrEmpty(User.DisplayName))
            {
                User.DisplayName = User.DisplayName.Replace("@", "").Replace(" ", "");
                if (!string.IsNullOrEmpty(User.DisplayName))
                {
                    HasError = false;
                }
            }
            else
            {
                HasError = true;
                Header = "not a validate user name";
            }
        }
        #endregion
    }
}

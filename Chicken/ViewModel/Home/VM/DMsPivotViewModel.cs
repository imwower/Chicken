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
        private LatestMessagesModel oldMessages;
        private LatestMessagesModel latestMessages;

        public DMsPivotViewModel()
        {
            Header = "Messages";
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
                oldMessages = new LatestMessagesModel();
            }
            else
            {
                oldMessages = file;
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
                            if (!latestMessages.Messages.ContainsKey(message.User.Id))
                            {
                                var user = new User
                                {
                                    Id = message.User.Id,
                                    DisplayName = message.User.DisplayName,
                                    ProfileImage = message.User.ProfileImage,
                                };
                                latestMessages.Messages.Add(message.User.Id, new Conversation { User = user });
                            }
                            latestMessages.Messages[message.User.Id].Messages.Add(message);
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
                    if (messages != null && messages.Count != 0)
                    {
                        latestMessages.SinceIdByMe = messages.First().Id;
                        latestMessages.MaxIdByMe = messages.Last().Id;

                        foreach (var message in messages)
                        {
                            message.IsSentByMe = true;
                            message.User = message.Receiver;
                            if (!oldMessages.Messages.ContainsKey(message.User.Id))
                            {
                                var user = new User
                                {
                                    Id = message.User.Id,
                                    DisplayName = message.User.DisplayName,
                                    ProfileImage = message.User.ProfileImage,
                                };
                                latestMessages.Messages.Add(message.User.Id, new Conversation { User = user });
                            }
                            latestMessages.Messages[message.User.Id].Messages.Add(message);
                        }
                    }

                    foreach (var kvp in latestMessages.Messages)
                    {
                        //no new msg:
                        //if (!kvp.Value.IsNew)
                        //{
                        //    TweetList.Add(new TweetViewModel(kvp.Value.Messages[0]));
                        //}
                        ////add new msgs to file:
                        //else
                        {
                            var msgs = kvp.Value.Messages.OrderByDescending(m => m.CreatedDate).ToList();
                            TweetList.Add(new TweetViewModel(msgs[0]));
                            //msgs.RemoveAt(0);
                            kvp.Value.Messages = msgs;
                            IsolatedStorageService.AddMessages(kvp.Value);
                        }
                    }
                    base.Refreshed();

                    IsolatedStorageService.CreateObject(Const.PageNameEnum.MainPage, Const.LATEST_MESSAGES_FILENAME, oldMessages);
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Status.VM
{
    public class StatusDetailViewModel : StatusViewModelBase
    {
        #region properties
        private TweetViewModel tweetViewModel;
        public TweetViewModel TweetViewModel
        {
            get
            {
                return tweetViewModel;
            }
            set
            {
                tweetViewModel = value;
                RaisePropertyChanged("TweetViewModel");
            }
        }

        private ObservableCollection<TweetViewModel> conversationList;
        public ObservableCollection<TweetViewModel> ConversationList
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

        public StatusDetailViewModel()
        {
            Header = "Detail";
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
            ClickHandler = this.ClickAction;
            ItemClickHandler = this.ItemClickAction;
            AddFavoriteHandler = this.AddFavoriteAction;
            RetweetHandler = this.RetweetAction;
            ReplyHandler = this.ReplyAction;
            QuoteHandler = this.QuoteAction;
        }

        private void RefreshAction()
        {
            GetResult<Tweet>(StatusId,
                tweet =>
                {
                    TweetViewModel = new TweetViewModel(tweet);
                    LoadConversation(tweet.InReplyToTweetId);
                    IsInited = true;
                });
        }

        private void LoadAction()
        {
            var tweet = ConversationList[ConversationList.Count - 1];
            LoadConversation(tweet.InReplyToTweetId);
        }

        private void ClickAction(object parameter)
        {
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.ProfilePage, parameter);
        }

        private void ItemClickAction(object parameter)
        {
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.StatusPage, parameter);
        }

        private void AddFavoriteAction()
        {
            var action = tweetViewModel.Favorited ? AddToFavoriteActionType.Destroy : AddToFavoriteActionType.Create;
            TweetService.AddToFavorites<Tweet>(StatusId, action,
                tweet =>
                {
                    List<ErrorMessage> errors = tweet.Errors;
                    if (errors != null && errors.Count != 0)
                    {
                        //error
                    }
                    Refresh();
                });
        }

        private void RetweetAction()
        {
            var action = RetweetActionType.Create;
            if (!tweetViewModel.Retweeted)
            {
                TweetService.Retweet<Tweet>(StatusId, action,
                   tweet =>
                   {
                       List<ErrorMessage> errors = tweet.Errors;
                       if (errors != null && errors.Count != 0)
                       {
                           //error
                       }
                       Refresh();
                   });
            }
        }

        private void ReplyAction()
        {
            DoAction(NewTweetActionType.Reply);
        }

        private void QuoteAction()
        {
            DoAction(NewTweetActionType.Quote);
        }

        #region private method
        private void DoAction(NewTweetActionType type)
        {
            if (IsLoading)
            {
                return;
            }
            NewTweetModel newTweet = new NewTweetModel
            {
                ActionType = (int)type,
            };
            switch (type)
            {
                case NewTweetActionType.Quote:
                    newTweet.Text = Const.QUOTECHARACTER + " " + tweetViewModel.User.ScreenName + " " + tweetViewModel.Text;
                    newTweet.InReplyToStatusId = tweetViewModel.Id;
                    newTweet.InReplyToUserScreenName = tweetViewModel.User.ScreenName;
                    break;
                case NewTweetActionType.Reply:
                    newTweet.Text = tweetViewModel.User.ScreenName + " ";
                    newTweet.InReplyToStatusId = tweetViewModel.Id;
                    newTweet.InReplyToUserScreenName = tweetViewModel.User.ScreenName;
                    break;
                case NewTweetActionType.PostNew:
                case NewTweetActionType.None:
                default:
                    break;
            }
            IsLoading = false;
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.NewTweetPage, newTweet);
        }

        private void LoadConversation(string statusId)
        {
            if (string.IsNullOrEmpty(statusId))
            {
                LoadHandler = null;
                base.Loaded();
                return;
            }
            if (ConversationList == null)
            {
                ConversationList = new ObservableCollection<TweetViewModel>();
            }
            GetResult<Tweet>(statusId,
                tweet =>
                {
                    ConversationList.Add(new TweetViewModel(tweet));
                    base.Loaded();
                });
        }

        private void GetResult<T>(string statusId, Action<T> action)
        {
            TweetService.GetStatusDetail<T>(statusId,
                tweet =>
                {
                    if (tweet != null && action != null)
                    {
                        action(tweet);
                    }
                });
        }
        #endregion
    }
}

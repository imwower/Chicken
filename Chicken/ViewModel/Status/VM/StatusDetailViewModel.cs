using System;
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
        }

        private void RefreshAction()
        {
            GetResult<Tweet>(StatusId,
                tweet =>
                {
                    TweetViewModel = new TweetViewModel(tweet);
                    LoadConversation(tweet.InReplyToTweetId);
                });
        }

        private void LoadAction()
        {
            if (!string.IsNullOrEmpty(tweetViewModel.InReplyToTweetId))
            {
                var tweet = ConversationList[ConversationList.Count - 1];
                LoadConversation(tweet.InReplyToTweetId);
            }
        }

        /// <summary>
        /// navigate to profile detail page
        /// </summary>
        /// <param name="parameter">user id</param>
        private void ClickAction(object parameter)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.USER_ID, parameter);
            NavigationServiceManager.NavigateTo(Const.ProfilePage, parameters);
        }

        private void ItemClickAction(object parameter)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.ID, parameter);
            NavigationServiceManager.NavigateTo(Const.StatusPage, parameters);
        }

        private void AddFavoriteAction(object parameter)
        {
            //TODO
        }

        #region private method
        private void LoadConversation(string statusId)
        {
            if (string.IsNullOrEmpty(statusId))
            {
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

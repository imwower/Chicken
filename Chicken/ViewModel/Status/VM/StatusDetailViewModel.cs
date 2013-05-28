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

        private TweetViewModel retweetViewModel;
        public TweetViewModel RetweetViewModel
        {
            get
            {
                return retweetViewModel;
            }
            set
            {
                retweetViewModel = value;
                RaisePropertyChanged("RetweetViewModel");
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
        }

        public override void Refresh()
        {
            GetResult<Tweet>(StatusId,
                tweet =>
                {
                    this.TweetViewModel = new TweetViewModel(tweet);
                    LoadConversation(tweet.InReplyToTweetId);
                    base.Refresh();
                });
        }

        public override void Load()
        {
            if (string.IsNullOrEmpty(this.tweetViewModel.InReplyToTweetId))
            {
                return;
            }
            var tweet = this.ConversationList[this.ConversationList.Count - 1];
            LoadConversation(tweet.InReplyToTweetId);
            base.Load();
        }

        /// <summary>
        /// navigate to profile detail page
        /// </summary>
        /// <param name="parameter">user id</param>
        public override void Click(object parameter)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>(1);
            parameters.Add(Const.USER_ID, parameter);
            NavigationServiceManager.NavigateTo(Const.ProfilePage, parameters);
        }

        public override void ItemClick(object parameter)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>(1);
            parameters.Add(Const.ID, parameter);
            NavigationServiceManager.NavigateTo(Const.StatusPage, parameters);
        }

        #region private method
        private void LoadConversation(string statusId)
        {
            if (string.IsNullOrEmpty(statusId))
            {
                base.Load();
                return;
            }
            if (this.ConversationList == null)
            {
                this.ConversationList = new ObservableCollection<TweetViewModel>();
            }
            GetResult<Tweet>(statusId,
                tweet =>
                {
                    this.ConversationList.Add(new TweetViewModel(tweet));
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

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Chicken.ViewModel.Home.Base;
using Chicken.Model;
using System.Collections.ObjectModel;

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
        }

        public override void Refresh()
        {
            GetResult<Tweet>(StatusId,
                tweet =>
                {
                    this.TweetViewModel = new TweetViewModel(tweet);
                    LoadConversation(tweet.InReplyToTweetId);
                    base.Refreshed();
                });
        }

        public override void Load()
        {
            if(string.IsNullOrEmpty(this.tweetViewModel.InReplyToTweetId))
            {
                return;
            }
            var tweet = this.ConversationList[this.ConversationList.Count-1];
            LoadConversation(tweet.InReplyToTweetId);
            base.Loaded();
        }

        private void LoadConversation(string statusId)
        {
            if (string.IsNullOrEmpty(statusId))
            {
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

        public override void Click(object parameter)
        {
            base.Click(parameter);
        }

        public override void ItemClick(object parameter)
        {
            base.ItemClick(parameter);
        }
    }
}

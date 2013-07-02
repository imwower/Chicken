using System.Collections.ObjectModel;
using Chicken.Model;
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
        }

        private void RefreshAction()
        {
            TweetViewModel = new TweetViewModel(Tweet);
            if (ConversationList != null && ConversationList.Count != 0)
            {
                ConversationList.Clear();
            }
            LoadConversation(Tweet.InReplyToTweetId);
            IsInited = true;
        }

        private void LoadAction()
        {
            string statusId = string.Empty;
            if (ConversationList != null && ConversationList.Count != 0)
            {
                statusId = ConversationList[ConversationList.Count - 1].InReplyToTweetId;
            }
            LoadConversation(statusId);
        }

        #region private method
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
            TweetService.GetStatusDetail<Tweet>(statusId,
                tweet =>
                {
                    ConversationList.Add(new TweetViewModel(tweet));
                    base.Loaded();
                });
        }
        #endregion
    }
}

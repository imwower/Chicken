using System.Collections.ObjectModel;
using Chicken.Model;
using Chicken.ViewModel.Base;

namespace Chicken.ViewModel.Status.VM
{
    public class StatusDetailViewModel : StatusViewModelBase
    {
        #region properties
        public ObservableCollection<TweetViewModel> ConversationList { get; set; }
        #endregion

        public StatusDetailViewModel()
        {
            ConversationList = new ObservableCollection<TweetViewModel>();
            RefreshHandler = this.RefreshAction;
            LoadHandler = this.LoadAction;
        }

        #region actions
        private void RefreshAction()
        {
            if (!CheckIfLoaded())
                return;
            ConversationList.Clear();
            LoadConversation(Tweet.InReplyToTweetId);
            IsInited = true;
        }

        private void LoadAction()
        {
            string statusId = string.Empty;
            if (ConversationList.Count != 0)
                statusId = ConversationList[ConversationList.Count - 1].InReplyToTweetId;
            LoadConversation(statusId);
        }
        #endregion

        #region private method
        private void LoadConversation(string statusId)
        {
            if (string.IsNullOrEmpty(statusId))
            {
                LoadHandler = null;
                base.Loaded();
                return;
            }
            TweetService.GetStatusDetail(statusId,
                tweet =>
                {
                    if (!tweet.HasError)
                        ConversationList.Add(new TweetViewModel(tweet));
                    base.Loaded();
                });
        }
        #endregion
    }
}

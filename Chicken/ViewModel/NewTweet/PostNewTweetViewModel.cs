using System.Windows.Input;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.NewTweet.Base;
using Chicken.Common;
using Chicken.Model;

namespace Chicken.ViewModel.NewTweet
{
    public class PostNewTweetViewModel : NotificationObject
    {
        #region properties
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }
        private NewTweetViewModel tweetViewModel;
        public NewTweetViewModel TweetViewModel
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
        #endregion

        #region binding
        public ICommand SendCommand
        {
            get
            {
                return new DelegateCommand(SendAction);
            }
        }

        public ICommand AddImageCommand
        {
            get
            {
                return new DelegateCommand(AddImageAction);
            }
        }

        public ICommand MentionCommand
        {
            get
            {
                return new DelegateCommand(MentionAction);
            }
        }

        public ICommand AddEmotionCommand
        {
            get
            {
                return new DelegateCommand(AddEmotionAction);
            }
        }
        #endregion

        #region service
        private ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public PostNewTweetViewModel()
        {
            Title = "what's happening?";
            tweetViewModel = new NewTweetViewModel
            {
                Text = "Hello World",
            };
        }

        #region private method
        private void SendAction()
        {
            var parameters = TwitterHelper.GetDictionary();
            TweetService.PostNewTweet<Tweet>(tweetViewModel.Text,
                tweet =>
                {
                    var error = tweet as ModelBase;
                    if (error.Errors != null && error.Errors.Count != 0)
                    {

                    }
                }, parameters);
        }

        private void AddImageAction()
        { }

        private void MentionAction()
        { }

        private void AddEmotionAction()
        { }

        #endregion
    }
}

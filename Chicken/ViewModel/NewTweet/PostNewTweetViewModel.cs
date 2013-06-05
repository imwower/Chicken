using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.NewTweet.Base;

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
            tweetViewModel = new NewTweetViewModel();
        }

        public void NavigateTo(object parameter = null)
        {
            if (parameter != null)
            {
                var tweet = parameter as NewTweetViewModel;
                TweetViewModel.ActionType = tweet.ActionType;
                switch (tweet.ActionType)
                {
                    case NewTweetActionType.Quote:
                        Title = "Quote:";
                        TweetViewModel.Text = Const.QUOTECHARACTER + " " + tweet.InReplyToUserScreenName + " " + tweet.Text;
                        TweetViewModel.InReplyToStatusId = tweet.InReplyToStatusId;
                        break;
                    case NewTweetActionType.Reply:
                        Title = "Reply To:";
                        TweetViewModel.InReplyToStatusId = tweet.InReplyToStatusId;
                        TweetViewModel.Text = tweet.InReplyToUserScreenName + " ";
                        break;
                    case NewTweetActionType.PostNew:
                    case NewTweetActionType.None:
                    default:
                        TweetViewModel.Text = tweet.Text;
                        break;
                }
            }
        }

        public void AddImageStream(string fileName, Stream stream)
        {
            if (stream == null)
            {
                return;
            }
            tweetViewModel.MediaStream = stream;
            tweetViewModel.FileName = fileName;
        }

        #region private method
        private void SendAction()
        {
            if (tweetViewModel.Text.Length == 0)
            {
                return;
            }
            TweetService.PostNewTweet<Tweet>(tweetViewModel,
                tweet =>
                {
                    List<ErrorMessage> errors = tweet.Errors;
                    if (errors != null && errors.Count != 0)
                    {
                        //error;
                    }
                    else
                    {
                        NavigationServiceManager.NavigateTo(Const.MainPage);
                    }
                });
        }

        private void MentionAction()
        { }

        private void AddEmotionAction()
        { }

        #endregion
    }
}

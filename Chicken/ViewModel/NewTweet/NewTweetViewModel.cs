using System;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;

namespace Chicken.ViewModel.NewTweet
{
    public class NewTweetViewModel : PivotItemViewModelBase
    {
        #region event handler
        public Action InitHandler;
        public Action BeforeSendHandler;
        public Action AddEmotionHandler;
        public Action KeyboardHandler;
        #endregion

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
        public NewTweetModel TweetModel { get; set; }
        public virtual string Text
        {
            get
            {
                return TweetModel.Text;
            }
            set
            {
                TweetModel.Text = value;
                RaisePropertyChanged("Text");
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
        #endregion

        #region binding
        public ICommand SendCommand
        {
            get
            {
                return new DelegateCommand(SendAction);
            }
        }

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

        public NewTweetViewModel()
        {
            TweetModel = new NewTweetModel();
            RefreshHandler = this.RefreshAction;
        }

        #region actions
        private void RefreshAction()
        {
            var file = IsolatedStorageService.GetAndDeleteObject<NewTweetModel>(Const.NewTweetPage);
            if (file != null)
            {
                TweetModel = file;
                RaisePropertyChanged("Text");
            }
            switch (TweetModel.Type)
            {
                case NewTweetActionType.Quote:
                    Title = LanguageHelper.GetString("NewTweetPage_Header_Quote");
                    break;
                case NewTweetActionType.Reply:
                    Title = LanguageHelper.GetString("NewTweetPage_Header_ReplyTo", TweetModel.InReplyToUserScreenName);
                    break;
                case NewTweetActionType.Mention:
                    Title = LanguageHelper.GetString("NewTweetPage_Header_Mention", TweetModel.InReplyToUserScreenName);
                    break;
                case NewTweetActionType.PostNew:
                case NewTweetActionType.None:
                default:
                    Title = LanguageHelper.GetString("NewTweetPage_Header_WhatsHappening");
                    break;
            }
            if (InitHandler != null)
                InitHandler();
            base.Refreshed();
        }

        protected virtual void SendAction()
        {
            if (IsLoading || string.IsNullOrEmpty(TweetModel.Text))
                return;
            IsLoading = true;
            if (BeforeSendHandler != null)
                BeforeSendHandler();
            TweetService.PostNewTweet(TweetModel,
                tweet =>
                {
                    IsLoading = false;
                    if (tweet.HasError)
                        return;
                    TweetModel.Text = string.Empty;
                    App.HandleMessage(new ToastMessage
                    {
                        Message = LanguageHelper.GetString("Toast_Msg_TweetSentSuccessfully"),
                        Complete = () => NavigationServiceManager.NavigateTo(Const.HomePage)
                    });
                });
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
    }
}

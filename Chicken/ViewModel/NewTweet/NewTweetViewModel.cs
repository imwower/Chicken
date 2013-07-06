using System.Collections.Generic;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;

namespace Chicken.ViewModel.NewTweet
{
    public class NewTweetViewModel : PivotItemViewModelBase
    {
        #region event handler
        public delegate void AddEmotionEventHandler();
        public AddEmotionEventHandler BeforeSendHandler;
        public AddEmotionEventHandler AddEmotionHandler;
        public AddEmotionEventHandler KeyboardHandler;
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

        #region service
        private ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public NewTweetViewModel()
        {
            Title = "what's happening?";
            TweetModel = new NewTweetModel();
        }

        #region actions
        protected virtual void SendAction()
        {
            if (IsLoading
                || string.IsNullOrEmpty(TweetModel.Text))
                return;
            IsLoading = true;
            if (BeforeSendHandler != null)
                BeforeSendHandler();
            TweetService.PostNewTweet(TweetModel,
                tweet =>
                {
                    IsLoading = false;
                    List<ErrorMessage> errors = tweet.Errors;
                    if (errors != null && errors.Count != 0)
                    {
                        HandleMessage(new ToastMessage
                        {
                            Message = errors[0].Message
                        });
                        return;
                    }
                    TweetModel.Text = string.Empty;
                    HandleMessage(new ToastMessage
                    {
                        Message = "tweet sent successfully",
                        Complete =
                        () =>
                        {
                            NavigationServiceManager.NavigateTo(PageNameEnum.HomePage);
                        }
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

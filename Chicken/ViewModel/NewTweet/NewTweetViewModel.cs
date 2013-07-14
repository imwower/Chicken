﻿using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;

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

        public NewTweetViewModel()
        {
            Title = LanguageHelper.GetString("NewTweetPage_Header_WhatsHappening");
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

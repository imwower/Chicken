using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.NewTweet;
using Chicken.ViewModel;

namespace Chicken.View
{
    public partial class NewTweetPage : PageBase
    {
        #region properties
        private NewTweetViewModel newTweetViewModel;
        #endregion

        public NewTweetPage()
        {
            InitializeComponent();
            newTweetViewModel = new NewTweetViewModel()
            {
                BeforeSendHandler = this.BeforeSendAction,
                AddEmotionHandler = this.AddEmotionAction,
                KeyboardHandler = this.KeyboardAction
            };
            this.DataContext = newTweetViewModel;
            this.Loaded += NewTweetPage_Loaded;
        }

        #region loaded
        private void NewTweetPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.TextContent.Focus();
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var tweet = IsolatedStorageService.GetAndDeleteObject<NewTweetModel>(PageNameEnum.NewTweetPage);
            if (tweet != null)
            {
                newTweetViewModel.TweetModel.Type = tweet.Type;
                switch (tweet.Type)
                {
                    case NewTweetActionType.Quote:
                        newTweetViewModel.Title = LanguageHelper.GetString("NewTweetPage_Header_Quote");
                        this.TextContent.Text = tweet.Text;
                        newTweetViewModel.TweetModel.InReplyToStatusId = tweet.InReplyToStatusId;
                        this.TextContent.Select(0, 0);
                        break;
                    case NewTweetActionType.Reply:
                        newTweetViewModel.Title = LanguageHelper.GetString("NewTweetPage_Header_ReplyTo", tweet.InReplyToUserScreenName);
                        this.TextContent.Text = tweet.Text;
                        newTweetViewModel.TweetModel.InReplyToStatusId = tweet.InReplyToStatusId;
                        this.TextContent.Select(this.TextContent.Text.Length, 0);
                        break;
                    case NewTweetActionType.Mention:
                        newTweetViewModel.Title = LanguageHelper.GetString("NewTweetPage_Header_Mention", tweet.InReplyToUserScreenName);
                        this.TextContent.Text = tweet.Text;
                        this.TextContent.Select(this.TextContent.Text.Length, 0);
                        break;
                    case NewTweetActionType.PostNew:
                    case NewTweetActionType.None:
                    default:
                        this.TextContent.Text = tweet.Text;
                        this.TextContent.Select(this.TextContent.Text.Length, 0);
                        break;
                }
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (!string.IsNullOrEmpty(newTweetViewModel.TweetModel.Text))
            {
                IsolatedStorageService.CreateObject(PageNameEnum.NewTweetPage, newTweetViewModel.TweetModel);
            }
        }

        #region text content
        private void TextContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            //counter:
            int remain = Const.MaxCharLength - this.TextContent.Text.Length;
            if (remain < 0)
            {
                this.TextCounter.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.newTweetViewModel.TweetModel.Text = this.TextContent.Text;
            }
            this.TextCounter.Text = remain.ToString();
        }

        private void TextContent_GotFocus(object sender, RoutedEventArgs e)
        {
            (App.Current as App).RootFrame.RenderTransform = null;
            this.Emotions.Visibility = Visibility.Collapsed;
            this.newTweetViewModel.State = AppBarState.Default;
        }
        #endregion

        #region add image
        //private void AddImageButton_Click(object sender, EventArgs e)
        //{
        //    popup = new Popup();
        //    var photoChooserControl = new PhotoChooserControl();
        //    photoChooserControl.AddImageHandler = this.AddImageStream;
        //    photoChooserControl.CancelAddImageHandler = this.CancelAddImage;
        //    popup.Child = photoChooserControl;
        //    ClosePopup(false);
        //}

        //private void AddImageStream(string fileName, Stream stream)
        //{
        //    newTweetViewModel.TweetModel.FileName = fileName;
        //    ClosePopup(true);
        //}

        //private void CancelAddImage()
        //{
        //    ClosePopup(true);
        //}

        //private void ClosePopup(bool close)
        //{
        //    //this.IsHitTestVisible = ApplicationBar.IsVisible = close;
        //    //popup.IsOpen = !close;
        //}
        #endregion

        #region add emotion
        private void BeforeSendAction()
        {
            this.Focus();
        }

        private void AddEmotionAction()
        {
            if (!this.Emotions.IsInit)
            {
                this.Emotions.Init();
                this.Emotions.AddEmotion = this.AddEmotion;
            }
            this.Emotions.Visibility = Visibility.Visible;
            this.Focus();
        }

        private void AddEmotion(string emotion)
        {
            if (this.TextContent.Text.Length + emotion.Length > Const.MaxCharLength)
                return;
            int start = this.TextContent.SelectionStart;
            this.TextContent.Text = this.TextContent.Text.Insert(start, emotion);
            this.TextContent.SelectionStart = start + emotion.Length;
        }

        private void KeyboardAction()
        {
            this.Emotions.Visibility = Visibility.Collapsed;
            this.UpdateLayout();
            this.TextContent.Focus();
        }
        #endregion
    }
}
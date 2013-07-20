using System.Windows;
using System.Windows.Navigation;
using Chicken.Common;
using Chicken.Service;
using Chicken.ViewModel.NewTweet;

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
                InitHandler = this.InitAction,
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
            if (!newTweetViewModel.IsInited)
                newTweetViewModel.Refresh();
        }

        private void InitAction()
        {
            switch (newTweetViewModel.TweetModel.Type)
            {
                case NewTweetActionType.Quote:
                    this.TextContent.Select(0, 0);
                    break;
                case NewTweetActionType.Reply:
                    this.TextContent.Select(this.TextContent.Text.Length, 0);
                    break;
                case NewTweetActionType.Mention:
                    this.TextContent.Select(this.TextContent.Text.Length, 0);
                    break;
                case NewTweetActionType.PostNew:
                case NewTweetActionType.None:
                default:
                    break;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (!string.IsNullOrEmpty(newTweetViewModel.TweetModel.Text))
                IsolatedStorageService.CreateObject(Const.NewTweetPage, newTweetViewModel.TweetModel);
        }
        #endregion

        #region text content
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
            if (string.IsNullOrEmpty(this.TextContent.Text))
            {
                this.TextContent.Text = emotion;
                this.TextContent.SelectionStart = emotion.Length;
                return;
            }
            if (this.TextContent.Text.Length + emotion.Length > this.TextContent.MaxLength)
                return;
            int start = this.TextContent.SelectionStart;
            this.TextContent.Text = this.TextContent.Text.Insert(start, emotion);
            this.TextContent.SelectionStart = start + emotion.Length;
        }

        private void KeyboardAction()
        {
            this.Emotions.Visibility = Visibility.Collapsed;
            this.TextContent.Focus();
        }
        #endregion
    }
}
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.NewTweet;
using Microsoft.Phone.Controls;

namespace Chicken.View
{
    public partial class NewTweetPage : PhoneApplicationPage
    {
        #region properties
        private NewTweetViewModel newTweetViewModel;
        private bool alreadyInitEmotionPanel;
        #endregion

        public NewTweetPage()
        {
            InitializeComponent();
            this.BackKeyPress += new EventHandler<CancelEventArgs>(NewTweetPage_BackKeyPress);
            this.Loaded += new RoutedEventHandler(NewTweetPage_Loaded);
            newTweetViewModel = new NewTweetViewModel();
            newTweetViewModel.AddEmotionHandler = this.AddEmotionHandler;
            newTweetViewModel.KeyboardHandler = this.KeyboardHandler;
            this.DataContext = newTweetViewModel;
        }

        #region loaded
        private void NewTweetPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.TextContent.Focus();
        }

        private void NewTweetPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (popup != null && popup.IsOpen)
            //{
            //    ClosePopup(true);
            //    e.Cancel = true;
            //}
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var tweet = IsolatedStorageService.GetAndDeleteObject<NewTweetModel>(Const.PageNameEnum.NewTweetPage);
            if (tweet != null)
            {
                newTweetViewModel.TweetModel.Type = tweet.Type;
                switch (tweet.Type)
                {
                    case NewTweetActionType.Quote:
                        newTweetViewModel.Title = "Quote:";
                        this.TextContent.Text = tweet.Text;
                        newTweetViewModel.TweetModel.InReplyToStatusId = tweet.InReplyToStatusId;
                        this.TextContent.Select(0, 0);
                        break;
                    case NewTweetActionType.Reply:
                        newTweetViewModel.Title = "Reply To: " + tweet.InReplyToUserScreenName;
                        this.TextContent.Text = tweet.Text;
                        newTweetViewModel.TweetModel.InReplyToStatusId = tweet.InReplyToStatusId;
                        this.TextContent.Select(this.TextContent.Text.Length, 0);
                        break;
                    case NewTweetActionType.Mention:
                        newTweetViewModel.Title = "Mention: " + tweet.InReplyToUserScreenName;
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
                IsolatedStorageService.CreateObject(Const.PageNameEnum.NewTweetPage, newTweetViewModel.TweetModel);
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
            this.EmotionPanel.Visibility = Visibility.Collapsed;
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
        private void AddEmotionHandler()
        {
            if (!alreadyInitEmotionPanel)
            {
                alreadyInitEmotionPanel = true;
                var emotions = IsolatedStorageService.GetEmotions();
                foreach (var emotion in emotions)
                {
                    Button button = new Button { Content = emotion };
                    button.Click += new RoutedEventHandler(Button_Click);
                    this.EmotionPanel.Children.Add(button);
                }
            }
            this.EmotionPanel.Visibility = Visibility.Visible;
            this.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string result = button.Content.ToString();
            if (this.TextContent.Text.Length + result.Length > Const.MaxCharLength)
            {
                return;
            }
            int start = this.TextContent.SelectionStart;
            this.TextContent.Text = this.TextContent.Text.Insert(start, result);
            this.TextContent.SelectionStart = start + result.Length;
        }

        private void KeyboardHandler()
        {
            this.EmotionPanel.Visibility = Visibility.Collapsed;
            this.TextContent.Focus();
        }
        #endregion
    }
}
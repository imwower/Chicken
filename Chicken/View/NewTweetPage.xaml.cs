using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Navigation;
using Chicken.Common;
using Chicken.Controls;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.NewTweet;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Chicken.View
{
    public partial class NewTweetPage : PhoneApplicationPage
    {
        #region properties
        private const int maxLength = 400;
        private const int maxCharLength = 140;
        private NewTweetViewModel newTweetViewModel;
        private Popup popup;
        #endregion

        public NewTweetPage()
        {
            InitializeComponent();
            this.BackKeyPress += new EventHandler<CancelEventArgs>(NewTweetPage_BackKeyPress);
            this.Loaded += new RoutedEventHandler(NewTweetPage_Loaded);
            newTweetViewModel = new NewTweetViewModel();
            this.DataContext = newTweetViewModel;
        }

        #region loaded
        private void NewTweetPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.TextContent.Focus();
        }

        private void NewTweetPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (popup != null && popup.IsOpen)
            {
                ClosePopup(true);
                e.Cancel = true;
            }
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var tweet = IsolatedStorageService.GetAndDeleteObject<NewTweetModel>(Const.PageNameEnum.NewTweetPage);
            if (tweet != null)
            {
                newTweetViewModel.TweetModel.ActionType = tweet.ActionType;
                switch ((NewTweetActionType)tweet.ActionType)
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
            int remain = maxCharLength - this.TextContent.Text.Length;
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
            (App.Current as App).RootFrame.RenderTransform = new CompositeTransform();
        }
        #endregion

        #region add image
        private void AddImageButton_Click(object sender, EventArgs e)
        {
            popup = new Popup();
            var photoChooserControl = new PhotoChooserControl();
            photoChooserControl.AddImageHandler = this.AddImageStream;
            photoChooserControl.CancelAddImageHandler = this.CancelAddImage;
            popup.Child = photoChooserControl;
            ClosePopup(false);
        }

        private void AddImageStream(string fileName, Stream stream)
        {
            newTweetViewModel.TweetModel.FileName = fileName;
            ClosePopup(true);
        }

        private void CancelAddImage()
        {
            ClosePopup(true);
        }

        private void ClosePopup(bool close)
        {
            this.IsHitTestVisible = ApplicationBar.IsVisible = close;
            popup.IsOpen = !close;
        }
        #endregion
    }
}
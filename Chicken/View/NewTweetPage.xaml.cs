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
using Chicken.ViewModel.NewTweet;
using Chicken.ViewModel.NewTweet.Base;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Chicken.View
{
    public partial class NewTweetPage : PhoneApplicationPage
    {
        #region properties
        private const int maxLength = 400;
        private const int maxCharLength = 140;
        private bool isInit;
        private PostNewTweetViewModel newTweetViewModel;
        private Popup popup;
        #endregion

        public NewTweetPage()
        {
            InitializeComponent();
            this.BackKeyPress += new EventHandler<CancelEventArgs>(NewTweetPage_BackKeyPress);
            this.Loaded += new RoutedEventHandler(NewTweetPage_Loaded);
            newTweetViewModel = new PostNewTweetViewModel();
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
                popup.IsOpen = false;
                this.IsHitTestVisible = ApplicationBar.IsVisible = true;
                e.Cancel = true;
            }
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (PhoneApplicationService.Current.State.ContainsKey(Const.NewTweetParam))
            {
                var newTweet = PhoneApplicationService.Current.State[Const.NewTweetParam];
                newTweetViewModel.NavigateTo(newTweet);
                PhoneApplicationService.Current.State.Remove(Const.NewTweetParam);
            }
            else
            {
                newTweetViewModel.NavigateTo();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (!string.IsNullOrEmpty(newTweetViewModel.TweetViewModel.Text))
            {
                if (newTweetViewModel.TweetViewModel.MediaStream != null)
                {
                    newTweetViewModel.TweetViewModel.MediaStream.Close();
                    newTweetViewModel.TweetViewModel.MediaStream.Dispose();
                    newTweetViewModel.TweetViewModel.MediaStream = null;
                }
                PhoneApplicationService.Current.State.Add(Const.NewTweetParam, newTweetViewModel.TweetViewModel);
            }
        }

        #region text content
        private void TextContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            var exp = this.TextContent.GetBindingExpression(TextBox.TextProperty);
            exp.UpdateSource();
            //counter:
            int remain = maxCharLength - this.TextContent.Text.Length;
            if (remain < 0)
            {
                this.TextCounter.Foreground = new SolidColorBrush(Colors.Red);
            }
            this.TextCounter.Text = remain.ToString();

            #region Init
            if (!isInit)
            {
                switch (newTweetViewModel.TweetViewModel.ActionType)
                {
                    case NewTweetActionType.Quote:
                        this.TextContent.Select(0, 0);
                        break;
                    case NewTweetActionType.Reply:
                    default:
                        this.TextContent.Select(this.TextContent.Text.Length, 0);
                        break;
                }
                this.TextContent.Focus();
                isInit = true;
            }
            #endregion
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
            popup.Child = photoChooserControl;
            this.IsHitTestVisible = ApplicationBar.IsVisible = false;
            popup.IsOpen = true;
        }

        private void AddImageStream(string fileName, Stream stream)
        {
            newTweetViewModel.AddImageStream(fileName, stream);
            popup.IsOpen = false;
            this.IsHitTestVisible = ApplicationBar.IsVisible = true;
        }
        #endregion
    }
}
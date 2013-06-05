using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;
using Chicken.Common;
using Chicken.ViewModel.NewTweet;
using Chicken.ViewModel.NewTweet.Base;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Controls.Primitives;
using Chicken.Controls;
using System.IO;
using System.ComponentModel;

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
            newTweetViewModel = new PostNewTweetViewModel();
            this.DataContext = newTweetViewModel;
        }

        void NewTweetPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (popup != null && popup.IsOpen)
            {
                popup.IsOpen = false;
                e.Cancel = true;
            }
        }

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
    }
}
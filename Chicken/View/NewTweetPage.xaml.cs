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

namespace Chicken.View
{
    public partial class NewTweetPage : PhoneApplicationPage
    {
        #region properties
        private const int maxLength = 400;
        private const int maxCharLength = 140;
        private bool isInit;
        private PostNewTweetViewModel newTweetViewModel;
        #endregion

        public NewTweetPage()
        {
            InitializeComponent();
            newTweetViewModel = new PostNewTweetViewModel();
            this.DataContext = newTweetViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (PhoneApplicationService.Current.State.ContainsKey(Const.NewTweetParam))
            {
                var newTweet = PhoneApplicationService.Current.State[Const.NewTweetPage];
                newTweetViewModel.NavigateTo(newTweet);
                PhoneApplicationService.Current.State.Remove(Const.NewTweetPage);
            }
            if (PhoneApplicationService.Current.State.ContainsKey(Const.ChosenPhotoStream))
            {
                var chosenPhotoStream = PhoneApplicationService.Current.State[Const.ChosenPhotoStream];
                newTweetViewModel.AddPhotoStream(chosenPhotoStream);
                PhoneApplicationService.Current.State.Remove(Const.NewTweetPage);
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
    }
}
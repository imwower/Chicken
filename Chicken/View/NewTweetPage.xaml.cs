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
        #region dp
        private const double LandscapeShift = 0d;
        private double topMargin = -480d;
        private const double LandscapeShift2 = -328d;
        private const double Epsilon = 0.00000001d;
        private const double PortraitShift = -339d;
        private const double PortraitShift2 = -408d;

        public static readonly DependencyProperty TranslateYProperty =
            DependencyProperty.Register("TranslateY", typeof(double), typeof(NewTweetPage), new PropertyMetadata(0d, OnRenderXPropertyChanged));
        public double TranslateY
        {
            get { return (double)GetValue(TranslateYProperty); }
            set { SetValue(TranslateYProperty, value); }
        }
        #endregion

        private bool isInit;
        private PostNewTweetViewModel newTweetViewModel;

        public NewTweetPage()
        {
            InitializeComponent();
            newTweetViewModel = new PostNewTweetViewModel();
            this.DataContext = newTweetViewModel;
            BindToKeyboardFocus();
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
            int remain = 140 - newTweetViewModel.TweetViewModel.Text.Length;
            if (remain < 0)
            {
                this.TextCounter.Foreground = new SolidColorBrush(Colors.Red);
            }
            this.TextCounter.Text = remain.ToString();
            if (this.ScrollView.VerticalOffset < this.ScrollView.ScrollableHeight)
            {
                this.ScrollView.ScrollToVerticalOffset(this.ScrollView.ScrollableHeight);
            }

            if (!isInit)
            {
                switch (newTweetViewModel.TweetViewModel.ActionType)
                {
                    case NewTweetActionType.Quote:
                        this.TextContent.Select(0, 0);
                        break;
                    case NewTweetActionType.Reply:
                        this.TextContent.Select(this.TextContent.Text.Length, 0);
                        break;
                    default:
                        break;
                }
                this.TextContent.Focus();
                isInit = true;
            }
        }

        private static void OnRenderXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((NewTweetPage)d).UpdateTopMargin((double)e.NewValue);
        }

        private void BindToKeyboardFocus()
        {
            PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (frame != null)
            {
                var group = frame.RenderTransform as TransformGroup;
                if (group != null)
                {
                    var translate = group.Children[0] as TranslateTransform;
                    var translateYBinding = new Binding("Y");
                    translateYBinding.Source = translate;
                    SetBinding(TranslateYProperty, translateYBinding);
                }
            }
        }

        private void UpdateTopMargin(double translateY)
        {
            if (translateY != topMargin)
            //if (IsClose(translateY, LandscapeShift) || IsClose(translateY, PortraitShift)
            //    || IsClose(translateY, LandscapeShift2) || IsClose(translateY, PortraitShift2))
            {
                this.LayoutRoot.Margin = new Thickness(0, -topMargin, 0, 0);
                this.topMargin = translateY;
            }
        }

        private bool IsClose(double a, double b)
        {
            return Math.Abs(a - b) < Epsilon;
        }

        private void TextContent_LostFocus(object sender, RoutedEventArgs e)
        {
            this.LayoutRoot.Margin = new Thickness();
        }
    }
}
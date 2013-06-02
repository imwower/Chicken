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
        private bool isInit;
        private PostNewTweetViewModel newTweetViewModel;

        public NewTweetPage()
        {
            InitializeComponent();
            newTweetViewModel = new PostNewTweetViewModel();
            this.DataContext = newTweetViewModel;
            //
            PhoneApplicationFrame frame = (App.Current as App).RootFrame;
            Binding b = new Binding("Y");
            b.Source = (frame.RenderTransform as TransformGroup).Children[0] as TranslateTransform;
            SetBinding(RootFrameTransformProperty, b);
        }

        public static readonly DependencyProperty RootFrameTransformProperty = DependencyProperty.Register(
            "RootFrameTransform",
            typeof(double),
            typeof(NewTweetPage),
            new PropertyMetadata(OnRootFrameTransformChanged));

       static void OnRootFrameTransformChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            double newvalue = (double)e.NewValue;
            NewTweetPage page = source as NewTweetPage;
            if (newvalue < 0.0)
            {
                page.LayoutRoot.Margin = new Thickness(0, -newvalue, 0, 0);
                //page.grid.Height = -newvalue;
            }
            else if (newvalue == 0.0)
            {
                page.LayoutRoot.Margin = new Thickness(0, 0, 0, 0);
                //page.grid.Height = 0;
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
            //if (this.ScrollView.VerticalOffset < this.ScrollView.ScrollableHeight)
            //{
            //    this.ScrollView.ScrollToVerticalOffset(this.ScrollView.ScrollableHeight);
            //}

            #region MyRegion
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
            #endregion
        }

        private void TextContent_LostFocus(object sender, RoutedEventArgs e)
        {
            //this.grid.Height = 0;
        }
    }
}
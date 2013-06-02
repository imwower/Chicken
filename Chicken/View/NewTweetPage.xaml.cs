using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Chicken.ViewModel.NewTweet;
using System.Windows.Navigation;

namespace Chicken.View
{
    public partial class NewTweetPage : PhoneApplicationPage
    {
        private PostNewTweetViewModel newTweetViewModel;

        public NewTweetPage()
        {
            InitializeComponent();
            newTweetViewModel = new PostNewTweetViewModel();
            this.DataContext = newTweetViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void TextContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            var exp = textBox.GetBindingExpression(TextBox.TextProperty);
            exp.UpdateSource();
        }
    }
}
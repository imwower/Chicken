using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chicken.Common;
using Chicken.ViewModel.NewMessage;

namespace Chicken.View
{
    public partial class NewMessagePage : PageBase
    {
        private NewMessageViewModel newMessageViewModel;

        public NewMessagePage()
        {
            InitializeComponent();
            newMessageViewModel = new NewMessageViewModel()
            {
                BeforeSendHandler = this.BeforeSendAction,
                AddEmotionHandler = this.AddEmotionAction,
                KeyboardHandler = this.KeyboardAction
            };
            this.DataContext = newMessageViewModel;
            this.Loaded += NewMessagePage_Loaded;
        }

        #region navigate to
        private void NewMessagePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!newMessageViewModel.IsInited)
            {
                newMessageViewModel.Refresh();
            }
        }
        #endregion

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
                var textbox = sender as TextBox;
                var binding = textbox.GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
            }
            this.TextCounter.Text = remain.ToString();
        }

        private void TextContent_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Emotions.Visibility = Visibility.Collapsed;
            this.newMessageViewModel.State = AppBarState.Default;
        }
        #endregion

        #region actions
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

        #region user name
        private void UserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.TextContent.Focus();
                newMessageViewModel.ValidateUser();
            }
        }

        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            var binding = textbox.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
        #endregion
    }
}
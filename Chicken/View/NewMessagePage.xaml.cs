using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
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
                ToastMessageHandler = ToastMessageHandler,
                AddEmotionHandler = this.AddEmotionHandler,
                KeyboardHandler = this.KeyboardHandler
            };
            this.DataContext = newMessageViewModel;
            this.Loaded += NewMessagePage_Loaded;
        }

        #region navigate to
        void NewMessagePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!newMessageViewModel.IsInited)
            {
                newMessageViewModel.Refresh();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var newMessage = IsolatedStorageService.GetAndDeleteObject<NewMessageModel>(Const.PageNameEnum.NewMessagePage);
            if (newMessage != null)
            {
                newMessageViewModel.NewMessage = newMessage;
            }
            else
            {
                newMessageViewModel.IsNew = true;
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
            this.UpdateLayout();
            this.newMessageViewModel.State = AppBarState.Default;
        }
        #endregion

        #region add emotion
        private void AddEmotionHandler()
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
            {
                return;
            }
            int start = this.TextContent.SelectionStart;
            this.TextContent.Text = this.TextContent.Text.Insert(start, emotion);
            this.TextContent.SelectionStart = start + emotion.Length;
        }

        private void KeyboardHandler()
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
                newMessageViewModel.ValidateFriendship();
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
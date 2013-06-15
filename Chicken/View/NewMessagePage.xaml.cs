using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.ViewModel.NewMessage;
using Microsoft.Phone.Controls;
using Chicken.Controls;

namespace Chicken.View
{
    public partial class NewMessagePage : PhoneApplicationPage
    {
        private NewMessageViewModel newMessageViewModel;
        private bool alreadyInitEmotionPanel;

        public NewMessagePage()
        {
            InitializeComponent();
            newMessageViewModel = new NewMessageViewModel();
            newMessageViewModel.ErrorHandler = this.ErrorHandler;
            newMessageViewModel.AddEmotionHandler = this.AddEmotionHandler;
            newMessageViewModel.KeyboardHandler = this.KeyboardHandler;
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (newMessageViewModel.NewMessage.Text != null)
            {
                IsolatedStorageService.CreateObject(Const.PageNameEnum.NewMessagePage, newMessageViewModel.NewMessage);
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
            this.EmotionPanel.Visibility = Visibility.Collapsed;
            this.UpdateLayout();
            this.newMessageViewModel.State = AppBarState.Default;
        }
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
            this.EmotionPanel.UpdateLayout();
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
            }
        }

        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            var binding = textbox.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
        #endregion

        #region error handler
        private void ErrorHandler(ErrorMessage message)
        {
            var toast = new ToastPrompt
            {
                Message = message.Message,
                TextWrapping = TextWrapping.Wrap,
            };
            toast.Show();
        }
        #endregion
    }
}
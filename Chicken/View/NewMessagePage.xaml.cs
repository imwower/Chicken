using System.Windows;
using System.Windows.Input;
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
                InitHandler = this.InitAction,
                BeforeSendHandler = this.BeforeSendAction,
                AddEmotionHandler = this.AddEmotionAction,
                KeyboardHandler = this.KeyboardAction
            };
            this.DataContext = newMessageViewModel;
            this.Loaded += NewMessagePage_Loaded;
        }

        #region load
        private void NewMessagePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!newMessageViewModel.IsInited)
                newMessageViewModel.Refresh();
        }
        #endregion

        #region actions
        private void InitAction()
        {
            if (newMessageViewModel.IsNew)
                this.UserName.Focus();
        }

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
            if (this.TextContent.Text.Length + emotion.Length > this.TextContent.MaxLength)
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

        #region text content
        private void TextContent_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Emotions.Visibility = Visibility.Collapsed;
            this.newMessageViewModel.State = AppBarState.Default;
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
        #endregion
    }
}
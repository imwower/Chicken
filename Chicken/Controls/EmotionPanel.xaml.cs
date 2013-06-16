using System.Windows;
using System.Windows.Controls;
using Chicken.Service;

namespace Chicken.Controls
{
    public partial class EmotionPanel : UserControl
    {
        private bool isInit;
        public bool IsInit
        {
            get
            {
                return isInit;
            }
        }
        public delegate void AddEmotionEventHandler(string emotion);
        public AddEmotionEventHandler AddEmotion;

        public EmotionPanel()
        {
            InitializeComponent();
        }

        public void Init()
        {
            if (isInit)
            {
                return;
            }
            var emotions = IsolatedStorageService.GetEmotions();
            foreach (var emotion in emotions)
            {
                Button button = new Button { Content = emotion };
                button.Click += new RoutedEventHandler(EmotionButton_Click);
                this.LayoutRoot.Children.Add(button);
            }
            this.UpdateLayout();
            isInit = true;
        }

        private void EmotionButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddEmotion != null)
            {
                string result = (sender as Button).Content.ToString();
                AddEmotion(result);
            }
        }
    }
}
using System;
using System.Windows;
using System.Windows.Controls;
using Chicken.Service;

namespace Chicken.Controls
{
    public partial class EmotionPanel : UserControl
    {
        #region properties
        private bool isInit;
        public bool IsInit
        {
            get
            {
                return isInit;
            }
        }
        public Action<string> AddEmotion;
        #endregion

        public EmotionPanel()
        {
            InitializeComponent();
        }

        public void Init()
        {
            if (isInit)
                return;
            var emotions = IsolatedStorageService.GetEmotions();
            foreach (var emotion in emotions)
            {
                var button = new Button { Content = emotion };
                button.Click += EmotionButton_Click;
                this.LayoutRoot.Children.Add(button);
            }
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
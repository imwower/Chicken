using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chicken.Controls
{
    public class AutoTextBox : TextBox
    {
        #region properties
        public static readonly Brush ErrorBrush = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
        public static readonly Brush ForegroundBrush = Application.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush;

        public static readonly DependencyProperty AssociatedTextBlockProperty =
            DependencyProperty.Register("AssociatedTextBlock", typeof(TextBlock), typeof(AutoTextBox), null);

        public TextBlock AssociatedTextBlock
        {
            get
            {
                return (TextBlock)GetValue(AssociatedTextBlockProperty);
            }
            set
            {
                SetValue(AssociatedTextBlockProperty, value);
            }
        }

        public static readonly DependencyProperty AllowOverFlowProperty =
            DependencyProperty.Register("AllowOverFlow", typeof(bool), typeof(AutoTextBox), null);

        public bool AllowOverFlow
        {
            get
            {
                return (bool)GetValue(AllowOverFlowProperty);
            }
            set
            {
                SetValue(AllowOverFlowProperty, value);
            }
        }
        #endregion

        public AutoTextBox()
        {
            this.TextChanged += AutoTextBox_TextChanged;
        }

        private void AutoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int remaining = this.MaxLength - this.Text.Length;
            if (AssociatedTextBlock != null)
                AssociatedTextBlock.Text = remaining.ToString();
            if (remaining < 0)
            {
                if (AssociatedTextBlock != null)
                    AssociatedTextBlock.Foreground = ErrorBrush;
                if (!AllowOverFlow)
                    return;
            }
            if (remaining >= 0 && AssociatedTextBlock != null)
                AssociatedTextBlock.Foreground = ForegroundBrush;
            var binding = GetBindingExpression(TextBox.TextProperty);
            if (binding != null)
                binding.UpdateSource();
        }
    }
}

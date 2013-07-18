using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chicken.Controls
{
    public class AutoTextBox : TextBox
    {
        #region private
        private const string IconBorderName = "IconBorder";
        private Border iconBorder;
        #endregion

        #region properties
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

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(AutoTextBox), null);

        public ImageSource Icon
        {
            get
            {
                return (ImageSource)GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }
        #endregion

        #region event handler
        public event EventHandler EnterKeyDown;
        #endregion

        public AutoTextBox()
        {
            this.TextChanged += AutoTextBox_TextChanged;
            this.KeyDown += AutoTextBox_KeyDown;
        }

        #region loaded
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (iconBorder != null)
                iconBorder.MouseLeftButtonDown -= Icon_Clicked;
            iconBorder = GetTemplateChild(IconBorderName) as Border;
            if (iconBorder != null)
                iconBorder.MouseLeftButtonDown += Icon_Clicked;
        }
        #endregion

        #region private method
        private void AutoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.MaxLength != 0)
            {
                int remaining = this.MaxLength - this.Text.Length;
                if (AssociatedTextBlock != null)
                    AssociatedTextBlock.Text = remaining.ToString();
                if (remaining < 0)
                {
                    if (AssociatedTextBlock != null)
                        AssociatedTextBlock.Foreground = App.PhoneAccentBrush;
                    if (!AllowOverFlow)
                        return;
                }
                if (remaining >= 0 && AssociatedTextBlock != null)
                    AssociatedTextBlock.Foreground = App.ForegroundBrush;
            }
            var binding = GetBindingExpression(TextBox.TextProperty);
            if (binding != null)
                binding.UpdateSource();
        }

        private void AutoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (EnterKeyDown != null)
                    EnterKeyDown(this, e);
            }
        }

        private void Icon_Clicked(object sender, MouseButtonEventArgs e)
        {
            var rootFrame = (Frame)Application.Current.RootVisual;
            rootFrame.Focus();

            if (EnterKeyDown != null)
                EnterKeyDown(this, e);
        }
        #endregion
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chicken.Controls
{
    public partial class AutoTextBox : UserControl
    {
        #region private
        private const string VisualStateFocused = "Focused";
        private const string VisualStateUnfocused = "Unfocused";
        #endregion

        #region properties
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AutoTextBox), null);

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(AutoTextBox), null);

        public int MaxLength
        {
            get
            {
                return (int)GetValue(MaxLengthProperty);
            }
            set
            {
                SetValue(MaxLengthProperty, value);
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

        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(AutoTextBox), null);

        public TextWrapping TextWrapping
        {
            get
            {
                return (TextWrapping)GetValue(TextWrappingProperty);
            }
            set
            {
                SetValue(TextWrappingProperty, value);
            }
        }

        public static readonly DependencyProperty SelectionStartProperty =
            DependencyProperty.Register("SelectionStart", typeof(int), typeof(AutoTextBox), null);

        public int SelectionStart
        {
            get
            {
                return (int)GetValue(SelectionStartProperty);
            }
            set
            {
                SetValue(SelectionStartProperty, value);
            }
        }

        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(AutoTextBox), null);

        public TextAlignment TextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(TextAlignmentProperty);
            }
            set
            {
                SetValue(TextAlignmentProperty, value);
            }
        }

        public static readonly DependencyProperty AcceptsReturnProperty =
            DependencyProperty.Register("AcceptsReturn", typeof(bool), typeof(AutoTextBox), null);

        public bool AcceptsReturn
        {
            get
            {
                return (bool)GetValue(AcceptsReturnProperty);
            }
            set
            {
                SetValue(AcceptsReturnProperty, value);
            }
        }

        public static readonly DependencyProperty InputScopeProperty =
            DependencyProperty.Register("InputScope", typeof(InputScope), typeof(AutoTextBox), null);

        public InputScope InputScope
        {
            get
            {
                return (InputScope)GetValue(InputScopeProperty);
            }
            set
            {
                SetValue(InputScopeProperty, value);
            }
        }
        #endregion

        #region event handler
        public event EventHandler EnterKeyDown;
        #endregion


        public AutoTextBox()
        {
            InitializeComponent();
            this.Loaded += AutoTextBox_Loaded;
        }

        public void Select(int start, int length)
        {
            this.ContentTextBox.Select(start, length);
        }

        #region loaded
        private void AutoTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.ContentTextBox.TextChanged += AutoTextBox_TextChanged;
            this.ContentTextBox.KeyDown += AutoTextBox_KeyDown;
            if (Icon == null)
                this.IconImage.Visibility = Visibility.Collapsed;
            else
                this.IconImage.MouseLeftButtonDown += IconBorder_MouseLeftButtonDown;
        }
        #endregion

        #region private method
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, VisualStateFocused, false);
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, VisualStateUnfocused, false);
            base.OnLostFocus(e);
        }

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
            Text = this.ContentTextBox.Text;
            var binding = GetBindingExpression(AutoTextBox.TextProperty);
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

        private void IconBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (EnterKeyDown != null)
                EnterKeyDown(this, e);
        }
        #endregion
    }
}

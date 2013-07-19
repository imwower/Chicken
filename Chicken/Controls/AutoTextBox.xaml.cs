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
        }

        public void Select(int start, int length)
        {
            this.ContentTextBox.Select(start, length);
        }

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
    }
}

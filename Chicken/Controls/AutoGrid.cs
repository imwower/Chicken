using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Chicken.Common;

namespace Chicken.Controls
{
    public class AutoGrid : Grid
    {
        public static readonly DependencyProperty ScrollToProperty =
        DependencyProperty.Register("ScrollTo", typeof(ScrollTo), typeof(AutoGrid), new PropertyMetadata(ScrollToPropertyChanged));

        public ScrollTo ScrollTo
        {
            get
            {
                return (ScrollTo)GetValue(ScrollToProperty);
            }
            set
            {
                SetValue(ScrollToProperty, value);
            }
        }

        private static void ScrollToPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var autoGrid = sender as AutoGrid;
            if (autoGrid != null && autoGrid.scrollViewer != null)
            {
                var newValue = (ScrollTo)e.NewValue;
                var oldValue = (ScrollTo)e.OldValue;
                if (newValue == oldValue)
                {
                    return;
                }
                switch (newValue)
                {
                    case ScrollTo.Top:
                        autoGrid.scrollViewer.ScrollToVerticalOffset(0);
                        break;
                    case ScrollTo.Bottom:
                        autoGrid.scrollViewer.ScrollToVerticalOffset(autoGrid.scrollViewer.ScrollableHeight);
                        break;
                    default:
                        break;
                }
            }
        }

        #region properties
        bool alreadyHookedScrollEvents = false;
        ScrollBar scrollBar;
        ScrollViewer scrollViewer;
        #endregion

        #region event
        public event EventHandler<EventArgs> VerticalCompressionTopHandler;
        public event EventHandler<EventArgs> VerticalCompressionBottomHandler;
        #endregion

        public AutoGrid()
        {
            this.Loaded += new RoutedEventHandler(AutoListBox_Loaded);
        }

        void AutoListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (alreadyHookedScrollEvents)
            {
                return;
            }
            alreadyHookedScrollEvents = true;
            scrollBar = Utils.FindVisualElement<ScrollBar>(this);
            scrollViewer = Utils.FindVisualElement<ScrollViewer>(this);
            if (scrollViewer != null)
            {
                FrameworkElement element = VisualTreeHelper.GetChild(scrollViewer, 0) as FrameworkElement;
                if (element != null)
                {
                    VisualStateGroup vgroup = Utils.FindVisualState(element, "VerticalCompression");
                    if (vgroup != null)
                    {
                        vgroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(vgroup_CurrentStateChanging);
                    }
                }
            }
        }

        private void vgroup_CurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Name == "CompressionTop")
            {
                if (VerticalCompressionTopHandler != null)
                {
                    VerticalCompressionTopHandler(sender, e);
                }
            }

            if (e.NewState.Name == "CompressionBottom")
            {
                if (VerticalCompressionBottomHandler != null)
                {
                    VerticalCompressionBottomHandler(sender, e);
                }
            }
            if (e.NewState.Name == "NoVerticalCompression")
            {

            }
        }
    }
}

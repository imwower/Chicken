using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Chicken.Controls
{
    public class AutoListBox : ListBox
    {
        bool alreadyHookedScrollEvents = false;
        ScrollBar scrollBar;
        ScrollViewer scrollViewer;

        public event EventHandler<EventArgs> VerticalCompressionTopHandler;
        public event EventHandler<EventArgs> VerticalCompressionBottomHandler;

        public AutoListBox()
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

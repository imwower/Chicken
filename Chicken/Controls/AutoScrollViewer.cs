using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Chicken.Service.Interface;
using System.Windows.Controls.Primitives;

namespace Chicken.Controls
{
    public class AutoScrollViewerPivotItem : PivotItem
    {
        bool alreadyHookedScrollEvents = false;
        //ScrollBar scrollBar;
        //ScrollViewer scrollViewer;

        public event EventHandler<EventArgs> VerticalCompressionTopHandler;
        public event EventHandler<EventArgs> VerticalCompressionBottomHandler;

        public AutoScrollViewerPivotItem()
        {
            this.Loaded += new RoutedEventHandler(AutoScrollViewerPage_Loaded);
        }

        void AutoScrollViewerPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (alreadyHookedScrollEvents)
            {
                return;
            }
            alreadyHookedScrollEvents = true;
            ScrollBar scrollBar = Utils.FindVisualElement<ScrollBar>(this);
            ScrollViewer scrollViewer = Utils.FindVisualElement<ScrollViewer>(this);
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

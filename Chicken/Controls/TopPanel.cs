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

namespace Chicken.Controls
{
    public class TopPanel : Control
    {
        private ScrollViewer scrollViewer;

        public TopPanel()
        {
            this.LayoutUpdated += new EventHandler(TopPanel_LayoutUpdated);
        }

        void TopPanel_LayoutUpdated(object sender, EventArgs e)
        {
            if (scrollViewer == null)
            {
                scrollViewer = Utils.FindVisualElement<ScrollViewer>(VisualTreeHelper.GetParent(this));
                if (scrollViewer != null)
                {
                    scrollViewer.MouseMove += new MouseEventHandler(scrollViewer_MouseMove);
                    scrollViewer.MouseLeftButtonUp += new MouseButtonEventHandler(scrollViewer_MouseLeftButtonUp);

                    if (scrollViewer.ManipulationMode != ManipulationMode.Control)
                    {
                        throw new InvalidOperationException("TopPanel requires the ScrollViewer to have ManipulationMode=Control. (ListBoxes may require re-templating.");
                    }
                }
            }
        }

        void scrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            UIElement scrollContent = (UIElement)scrollViewer.Content;
            CompositeTransform ct = scrollContent.RenderTransform as CompositeTransform;
            if (ct != null)
            {
 
            }
        }
        void scrollViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UIElement scrollContent = (UIElement)scrollViewer.Content;
            CompositeTransform ct = scrollContent.RenderTransform as CompositeTransform;
            if (ct != null)
            {

            }
        }


    }
}

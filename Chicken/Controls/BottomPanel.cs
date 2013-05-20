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
using System.Threading;

namespace Chicken.Controls
{
    public class BottomPanel : Control
    {
        double maxDistance = 70;
        bool needLoad;
        ScrollViewer scrollViewer;
        ProgressBar progessBar;
        Timer timer;

        public event EventHandler LoadRequested;

        public BottomPanel()
        {
            this.Loaded += new RoutedEventHandler(BottomPanel_Loaded);
        }

        void BottomPanel_Loaded(object sender, RoutedEventArgs e)
        {
            //scrollViewer = Utils.FindVisualElement<ScrollViewer>(VisualTreeHelper.GetParent(this));
            //if (scrollViewer != null)
            //{
            //    scrollViewer.MouseMove += new MouseEventHandler(scrollViewer_MouseMove);
            //    scrollViewer.MouseLeftButtonUp += new MouseButtonEventHandler(scrollViewer_MouseLeftButtonUp);

            //    if (scrollViewer.ManipulationMode != ManipulationMode.Control)
            //    {
            //        throw new InvalidOperationException("TopPanel requires the ScrollViewer to have ManipulationMode=Control. (ListBoxes may require re-templating.");
            //    }
            //}
            //progessBar = Utils.FindVisualElement<ProgressBar>(VisualTreeHelper.GetParent(this));
        }

        void scrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            //if (progessBar.Visibility == Visibility.Visible)
            //{
            //    return;
            //}
            UIElement scrollContent = (UIElement)scrollViewer.Content;
            CompositeTransform ct = scrollContent.RenderTransform as CompositeTransform;
            if (ct != null)
            {
                if (-ct.TranslateY > maxDistance)
                {
                    needLoad = true;
                }
                else
                {
                    needLoad = false;
                }
            }
        }
        void scrollViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if (progessBar.Visibility == Visibility.Visible ||
            //    !needLoad)
            //{
            //    return;
            //}
            UIElement scrollContent = (UIElement)scrollViewer.Content;
            CompositeTransform ct = scrollContent.RenderTransform as CompositeTransform;
            if (ct != null)
            {
                if (-ct.TranslateY > maxDistance)
                {
                    //progessBar.Visibility = Visibility.Visible;
                    if (LoadRequested != null)
                    {
                        timer = new Timer(
                            (obj) =>
                            {
                                Dispatcher.BeginInvoke(
                                    () =>
                                    {
                                        LoadRequested(sender, e);
                                        //progessBar.Visibility = Visibility.Collapsed;
                                    });
                            }, sender, 3000, -1);
                    }
                }
            }
        }

    }
}

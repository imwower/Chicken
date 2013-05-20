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
    public class TopPanel : Control
    {
        double maxDistance = 70;
        bool isRefreshing;
        ScrollViewer scrollViewer;
        ProgressBar progessBar;
        Timer timer;

        public event EventHandler RefreshRequested;

        public TopPanel()
        {
            this.Loaded += new RoutedEventHandler(TopPanel_Loaded);
        }

        void TopPanel_Loaded(object sender, RoutedEventArgs e)
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
            progessBar = Utils.FindVisualElement<ProgressBar>(VisualTreeHelper.GetParent(this));
        }

        void scrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRefreshing)
            {
                return;
            }
            UIElement scrollContent = (UIElement)scrollViewer.Content;
            CompositeTransform ct = scrollContent.RenderTransform as CompositeTransform;
            if (ct != null && !isRefreshing)
            {
                if (ct.TranslateY > maxDistance)
                {
                    isRefreshing = true;
                    progessBar.Visibility = Visibility.Visible;
                    if (RefreshRequested != null)
                    {
                        timer = new Timer(
                            (obj) =>
                            {
                                Dispatcher.BeginInvoke(
                                    () =>
                                    {
                                        RefreshRequested(sender, e);
                                        isRefreshing = false;
                                        progessBar.Visibility = Visibility.Collapsed;
                                    });
                            }, sender, 3000, -1);
                    }
                }
            }
        }
        void scrollViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UIElement scrollContent = (UIElement)scrollViewer.Content;
            CompositeTransform ct = scrollContent.RenderTransform as CompositeTransform;
            if (ct != null)
            {
                if (ct.TranslateY > maxDistance && isRefreshing)
                {
                    //    isRefreshing = true;
                    //    progessBar.Visibility = Visibility.Visible;
                    //    if (RefreshRequested != null)
                    //    {
                    //        RefreshRequested(sender, e);
                    //    }
                    //}
                    //else
                    //{
                    //isRefreshing = false;
                    //progessBar.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}

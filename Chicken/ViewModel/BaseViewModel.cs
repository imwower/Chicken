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
using System.Collections.Generic;

namespace Chicken.ViewModel
{
    public class BaseViewModel : NotificationObject
    {
        ScrollViewer scrollViewer;
        VisualStateGroup visualStateGroup;
        private string header;
        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
                RaisePropertyChanged("Header");
            }
        }
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        public BaseViewModel(DependencyObject container)
        {
            scrollViewer = FindVisualElement<ScrollViewer>(container);
            if (scrollViewer != null)
            {
                FrameworkElement element = VisualTreeHelper.GetChild(scrollViewer, 0) as FrameworkElement;
                if (element != null)
                {
                    visualStateGroup = FindVisualState(element, "ScrollStates");
                    visualStateGroup.CurrentStateChanged += VisualStateChanged;
                }
            }
        }

        public static T FindVisualElement<T>(DependencyObject container) where T : DependencyObject
        {
            var childQueue = new Queue<DependencyObject>();
            childQueue.Enqueue(container);
            while (childQueue.Count > 0)
            {
                var current = childQueue.Dequeue();
                T result = current as T;
                if (result != null && result != container)
                {
                    return result;
                }
                int childCount = VisualTreeHelper.GetChildrenCount(current);
                for (int childIndex = 0; childIndex < childCount; childIndex++)
                {
                    childQueue.Enqueue(VisualTreeHelper.GetChild(current, childIndex));
                }
            }
            return null;
        }

        public static VisualStateGroup FindVisualState(FrameworkElement element, string name)
        {
            if (element == null)
            {
                return null;
            }
            var groups = VisualStateManager.GetVisualStateGroups(element);
            foreach (VisualStateGroup group in groups)
            {
                if (group.Name == name)
                {
                    return group;
                }
            }
            return null;
        }

        public void VisualStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            var visualState = e.NewState.Name;
            switch (visualState)
            {
                case "Scrolling":
                    if (this.scrollViewer.ExtentHeight - this.scrollViewer.VerticalOffset <= this.scrollViewer.ViewportHeight * 1.1)
                    {
                        IsLoading = true;
                    }
                    break;
                case "NotScrolling":
                    if (this.scrollViewer.ExtentHeight - this.scrollViewer.VerticalOffset <= this.scrollViewer.ViewportHeight * 1.1)
                    {
                        HandleVisualStatueChangedPullUpEvent(sender, e);
                    }
                    IsLoading = false;
                    break;
                default:
                    IsLoading = false;
                    break;
            }
        }

        public delegate void VisualStateChangedHandler(object sender, VisualStateChangedEventArgs e);
        public VisualStateChangedHandler HandleVisualStatueChangedPullUpEvent;
    }
}

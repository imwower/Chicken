using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Chicken.Controls
{
    public static class Utils
    {
        #region UI method
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
        #endregion
    }
}

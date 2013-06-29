// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

// This is from the Silverlight Toolkit Nov 2011

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        /// <summary>
        /// Gets the VisualStateGroup with the given name, looking up the visual tree
        /// </summary>
        /// <param name="root">Element to start from</param>
        /// <param name="groupName">Name of the group to look for</param>
        /// <returns>The group, if found, or null</returns>
        public static VisualStateGroup GetVisualStateGroup(this FrameworkElement root, string groupName)
        {
            IEnumerable<FrameworkElement> selfOrAncestors = root.GetVisualAncestors().PrependWith(root);

            foreach (FrameworkElement element in selfOrAncestors)
            {
                IList groups = VisualStateManager.GetVisualStateGroups(element);
                foreach (object o in groups)
                {
                    VisualStateGroup group = o as VisualStateGroup;
                    if (group != null && group.Name == groupName)
                        return group;
                }
            }
            return null;
        }

        public static T GetFirstLogicalChildByType<T>(this FrameworkElement parent, bool applyTemplates)
            where T : FrameworkElement
        {
            var queue = new Queue<FrameworkElement>();
            queue.Enqueue(parent);

            while (queue.Count > 0)
            {
                var element = queue.Dequeue();
                var elementAsControl = element as Control;

                if (applyTemplates && elementAsControl != null)
                {
                    elementAsControl.ApplyTemplate();
                }

                if (element is T && element != parent)
                {
                    return (T)element;
                }

                foreach (var visualChild in element.GetVisualChildren().OfType<FrameworkElement>())
                {
                    queue.Enqueue(visualChild);
                }
            }
            return null;
        }

        public static IEnumerable<T> GetLogicalChildrenByType<T>(this FrameworkElement parent, bool applyTemplates)
            where T : FrameworkElement
        {
            if (applyTemplates && parent is Control)
            {
                ((Control)parent).ApplyTemplate();
            }

            var queue = new Queue<FrameworkElement>(parent.GetVisualChildren().OfType<FrameworkElement>());

            while (queue.Count > 0)
            {
                var element = queue.Dequeue();

                if (applyTemplates && element is Control)
                {
                    ((Control)element).ApplyTemplate();
                }

                if (element is T)
                {
                    yield return (T)element;
                }

                foreach (var visualChild in element.GetVisualChildren().OfType<FrameworkElement>())
                {
                    queue.Enqueue(visualChild);
                }
            }
        }

        public static IEnumerable<T> GetVisualAncestorsByType<T>(this FrameworkElement node, bool applyTemplates)
            where T : FrameworkElement
        {
            FrameworkElement parent = node.GetVisualParent();

            while (parent != null)
            {
                if (applyTemplates && parent is Control)
                {
                    ((Control)parent).ApplyTemplate();
                }

                if (parent is T)
                {
                    yield return parent as T;
                }

                parent = parent.GetVisualParent();
            }
        }

        /// <summary>
        /// Gets all the visual children of the element
        /// </summary>
        /// <param name="root">The element to get children of</param>
        /// <returns>An enumerator of the children</returns>
        public static IEnumerable<FrameworkElement> GetVisualChildren(this FrameworkElement root)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                yield return VisualTreeHelper.GetChild(root, i) as FrameworkElement;
        }

        /// <summary>
        /// Performs a breadth-first enumeration of all the descendents in the tree
        /// </summary>
        /// <param name="root">The root node</param>
        /// <returns>An enumerator of all the children</returns>
        public static IEnumerable<FrameworkElement> GetVisualDescendents(this FrameworkElement root)
        {
            Queue<IEnumerable<FrameworkElement>> toDo = new Queue<IEnumerable<FrameworkElement>>();

            toDo.Enqueue(root.GetVisualChildren());
            while (toDo.Count > 0)
            {
                IEnumerable<FrameworkElement> children = toDo.Dequeue();
                foreach (FrameworkElement child in children)
                {
                    yield return child;
                    toDo.Enqueue(child.GetVisualChildren());
                }
            }
        }

        /// <summary>
        /// Returns all the descendents of a particular type
        /// </summary>
        /// <typeparam name="T">The type to look for</typeparam>
        /// <param name="root">The root element</param>
        /// <param name="allAtSameLevel">Whether to stop searching the tree after the first set of items are found</param>
        /// <returns>List of the element found</returns>
        /// <remarks>
        /// The allAtSameLevel flag is used to control enumeration through the tree. For many cases (eg, finding ListBoxItems in a
        /// ListBox) you want enumeration to stop as soon as you've found all the items in the ListBox (no need to search further
        /// in the tree). For other cases though (eg, finding all the Buttons on a page) you want to exhaustively search the entire tree
        /// </remarks>
        public static IEnumerable<T> GetVisualDescendents<T>(this FrameworkElement root, bool allAtSameLevel) where T : FrameworkElement
        {
            bool found = false;
            foreach (FrameworkElement e in root.GetVisualDescendents())
            {
                if (e is T)
                {
                    found = true;
                    yield return e as T;
                }
                else
                {
                    if (found == true && allAtSameLevel == true)
                        yield break;
                }
            }
        }

        public static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
        {
            FrameworkElement parent = node.GetVisualParent();
            while (parent != null)
            {
                yield return parent;
                parent = parent.GetVisualParent();
            }
        }

        public static FrameworkElement GetVisualParent(this FrameworkElement node)
        {
            return VisualTreeHelper.GetParent(node) as FrameworkElement;
        }

        /// <summary>
        /// Prepends an item to the beginning of an enumeration
        /// </summary>
        /// <typeparam name="T">The type of item in the enumeration</typeparam>
        /// <param name="list">The existing enumeration</param>
        /// <param name="head">The item to return before the enumeration</param>
        /// <returns>An enumerator that returns the head, followed by the rest of the list</returns>
        public static IEnumerable<T> PrependWith<T>(this IEnumerable<T> list, T head)
        {
            yield return head;
            foreach (T item in list)
                yield return item;
        }

        /// <summary>
        /// Returns the items that are visible in a given container plus the invisible ones before and after.
        /// </summary>
        /// <remarks>This function assumes that items are ordered top-to-bottom or left-to-right; if items are in random positions it won't work</remarks>
        /// <typeparam name="T">The type of items being tested</typeparam>
        /// <param name="items">The items being tested; typically the children of a StackPanel</param>
        /// <param name="viewport">The viewport to test visibility against; typically a ScrollViewer</param>
        /// <param name="orientation">Wether to check for vertical or horizontal visibility</param>
        /// <param name="beforeItems">List to be populated with items that precede the visible items</param>
        /// <param name="visibleItems">List to be populated with the items that are visible</param>
        /// <param name="afterItems">List to be populated with the items that follow the visible items</param>
        public static void GetVisibleItems<T>(this IEnumerable<T> items, FrameworkElement viewport, Orientation orientation, out List<T> beforeItems, out List<T> visibleItems, out List<T> afterItems) where T : FrameworkElement
        {
            beforeItems = new List<T>();
            visibleItems = new List<T>();
            afterItems = new List<T>();

            VisibleSearchMode mode = VisibleSearchMode.Before;

            // Use a state machine to go over the enumertaion and populate the lists
            foreach (var item in items)
            {
                switch (mode)
                {
                    case VisibleSearchMode.Before:
                        if (item.TestVisibility(viewport, orientation, false))
                        {
                            beforeItems.Add(item);
                        }
                        else
                        {
                            visibleItems.Add(item);
                            mode = VisibleSearchMode.During;
                        }
                        break;

                    case VisibleSearchMode.During:
                        if (item.TestVisibility(viewport, orientation, true))
                        {
                            visibleItems.Add(item);
                        }
                        else
                        {
                            afterItems.Add(item);
                            mode = VisibleSearchMode.After;
                        }
                        break;

                    default:
                        afterItems.Add(item);
                        break;
                }
            }
        }

        /// <summary>
        /// Tests if the given item is visible or not inside a given viewport
        /// </summary>
        /// <param name="item">The item to check for visibility</param>
        /// <param name="viewport">The viewport to check visibility within</param>
        /// <param name="orientation">The orientation to check visibility with respect to (vertical or horizontal)</param>
        /// <param name="wantVisible">Whether the test is for being visible or invisible</param>
        /// <returns>True if the item's visibility matches the wantVisible parameter</returns>
        public static bool TestVisibility(this FrameworkElement item, FrameworkElement viewport, Orientation orientation, bool wantVisible)
        {
            // Determine the bounding box of the item relative to the viewport
            GeneralTransform transform = item.TransformToVisual(viewport);
            Point topLeft = transform.Transform(new Point(0, 0));
            Point bottomRight = transform.Transform(new Point(item.ActualWidth, item.ActualHeight));

            // Check for overlapping bounding box of the item vs. the viewport, depending on orientation
            double min, max, testMin, testMax;
            if (orientation == Orientation.Vertical)
            {
                min = topLeft.Y;
                max = bottomRight.Y;
                testMin = 0;
                testMax = Math.Min(viewport.ActualHeight, double.IsNaN(viewport.Height) ? double.PositiveInfinity : viewport.Height);
            }
            else
            {
                min = topLeft.X;
                max = bottomRight.X;
                testMin = 0;
                testMax = Math.Min(viewport.ActualWidth, double.IsNaN(viewport.Width) ? double.PositiveInfinity : viewport.Width);
            }

            bool result = wantVisible;

            if (min >= testMax || max <= testMin)
                result = !wantVisible;

#if false
      // Enable this to help with debugging if you are having issues...
      Debug.WriteLine(String.Format("Test visibility of {0}-{1} inside {2}-{3}. wantVisible {4}, result {5}",
        min, max, testMin, testMax, wantVisible, result));
#endif

            return result;
        }

        /// <summary>
        /// Simple enumeration used in the state machine of GetVisibleItems
        /// </summary>
        enum VisibleSearchMode
        {
            Before,
            During,
            After
        }
        #endregion

        #region Extension
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            Dictionary<TKey, object> keys = new Dictionary<TKey, object>();
            foreach (TSource element in source)
            {
                var elementValue = keySelector(element);
                if (!keys.ContainsKey(elementValue))
                {
                    keys.Add(elementValue, null);
                    yield return element;
                }
            }
        }
        #endregion
    }
}

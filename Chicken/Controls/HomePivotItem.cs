using System.Collections;
using System.Windows;
using Microsoft.Phone.Controls;

namespace Chicken.Controls
{
    public class HomePivotItem : PivotItem
    {
        public static readonly DependencyProperty ListBoxItemsProperty =
        DependencyProperty.Register("ListBoxItems", typeof(IEnumerable), typeof(HomePivotItem), null);

        public IEnumerable ListBoxItems
        {
            get { return (IEnumerable)GetValue(ListBoxItemsProperty); }
            set { SetValue(ListBoxItemsProperty, value); }
        }

        public static readonly DependencyProperty ListBoxItemTemplateProperty =
            DependencyProperty.Register("ListBoxItemTemplate", typeof(DataTemplate), typeof(HomePivotItem), null);

        public DataTemplate ListBoxItemTemplate
        {
            get { return (DataTemplate)GetValue(ListBoxItemTemplateProperty); }
            set { SetValue(ListBoxItemTemplateProperty, value); }
        }
    }
}

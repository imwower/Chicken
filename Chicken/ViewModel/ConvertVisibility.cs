using System;
using System.Windows;
using System.Windows.Data;

namespace Chicken.ViewModel
{
    public class ConvertVisibility : IValueConverter
    {
        public ConvertVisibility()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            bool s = (bool)value;
            return (s != true) ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

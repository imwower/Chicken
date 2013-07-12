using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Chicken.Controls
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        private static BooleanConverter converter = new BooleanConverter();

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return converter.ConvertBoolean(value);
        }

        protected object InvertConvertBoolean(object value)
        {
            return converter.ConvertBoolean(value, true);
        }

        protected object ConvertString(object value)
        {
            return converter.ConvertString(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private class BooleanConverter
        {
            public object ConvertBoolean(object value, bool invert = false)
            {
                if (value == null)
                    return Visibility.Collapsed;
                bool result = (bool)value;
                result = invert ? !result : result;
                return result ? Visibility.Visible : Visibility.Collapsed;
            }

            public object ConvertString(object value, bool invert = false)
            {
                if (value == null)
                    return Visibility.Collapsed;
                string s = value as string;
                if (string.IsNullOrEmpty(s) || s.Trim() == "0")
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }
    }

    public class InvertBooleanToVisibilityConverter : BooleanToVisibilityConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return base.InvertConvertBoolean(value);
        }
    }

    public class StringToVisibilityConverter : BooleanToVisibilityConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return base.ConvertString(value);
        }
    }

    public class BooleanToFillConverter : BooleanToVisibilityConverter
    {
        private static FillConverter converter = new FillConverter();

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return converter.Convert(value);
        }

        private class FillConverter
        {
            public object Convert(object value)
            {
                if (value == null)
                    return (Brush)Application.Current.Resources["PhoneForegroundBrush"];
                return (bool)value ?
                (Brush)Application.Current.Resources["PhoneAccentBrush"]
                : (Brush)Application.Current.Resources["PhoneForegroundBrush"];
            }
        }
    }
}

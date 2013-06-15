// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

// This is from the Silverlight Toolkit Nov 2011

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace Chicken.Controls
{
    public partial class WrapPanel : Panel
    {
        private bool _ignorePropertyChange;

        #region public double ItemHeight
        [TypeConverter(typeof(LengthConverter))]
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(
                "ItemHeight",
                typeof(double),
                typeof(WrapPanel),
                new PropertyMetadata(double.NaN, OnItemHeightOrWidthPropertyChanged));
        #endregion public double ItemHeight

        #region public double ItemWidth
        [TypeConverter(typeof(LengthConverter))]
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemWidthProperty =
    DependencyProperty.Register(
        "ItemWidth",
        typeof(double),
        typeof(WrapPanel),
        new PropertyMetadata(double.NaN, OnItemHeightOrWidthPropertyChanged));
        #endregion public double ItemWidth

        #region public Orientation Orientation

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
DependencyProperty.Register(
        "Orientation",
        typeof(Orientation),
        typeof(WrapPanel),
        new PropertyMetadata(Orientation.Horizontal, OnOrientationPropertyChanged));

        private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WrapPanel source = (WrapPanel)d;
            Orientation value = (Orientation)e.NewValue;

            if (source._ignorePropertyChange)
            {
                source._ignorePropertyChange = false;
                return;
            }

            if ((value != Orientation.Horizontal) &&
                (value != Orientation.Vertical))
            {
                source._ignorePropertyChange = true;
                source.SetValue(OrientationProperty, (Orientation)e.OldValue);

                throw new ArgumentException();
            }

            source.InvalidateMeasure();
        }
        #endregion public Orientation Orientation

        public WrapPanel()
        {
        }

        private static void OnItemHeightOrWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WrapPanel source = (WrapPanel)d;
            double value = (double)e.NewValue;

            if (source._ignorePropertyChange)
            {
                source._ignorePropertyChange = false;
                return;
            }

            if (!value.IsNaN() && ((value <= 0.0) || double.IsPositiveInfinity(value)))
            {
                source._ignorePropertyChange = true;
                source.SetValue(e.Property, (double)e.OldValue);

                throw new ArgumentException("value");
            }
            source.InvalidateMeasure();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Orientation o = Orientation;
            OrientedSize lineSize = new OrientedSize(o);
            OrientedSize totalSize = new OrientedSize(o);
            OrientedSize maximumSize = new OrientedSize(o, constraint.Width, constraint.Height);

            double itemWidth = ItemWidth;
            double itemHeight = ItemHeight;
            bool hasFixedWidth = !itemWidth.IsNaN();
            bool hasFixedHeight = !itemHeight.IsNaN();
            Size itemSize = new Size(
                hasFixedWidth ? itemWidth : constraint.Width,
                hasFixedHeight ? itemHeight : constraint.Height);

            foreach (UIElement element in Children)
            {
                element.Measure(itemSize);
                OrientedSize elementSize = new OrientedSize(
                    o,
                    hasFixedWidth ? itemWidth : element.DesiredSize.Width,
                    hasFixedHeight ? itemHeight : element.DesiredSize.Height);

                if (NumericExtensions.IsGreaterThan(lineSize.Direct + elementSize.Direct, maximumSize.Direct))
                {
                    totalSize.Direct = Math.Max(lineSize.Direct, totalSize.Direct);
                    totalSize.Indirect += lineSize.Indirect;

                    lineSize = elementSize;

                    if (NumericExtensions.IsGreaterThan(elementSize.Direct, maximumSize.Direct))
                    {
                        totalSize.Direct = Math.Max(elementSize.Direct, totalSize.Direct);
                        totalSize.Indirect += elementSize.Indirect;

                        lineSize = new OrientedSize(o);
                    }
                }
                else
                {
                    lineSize.Direct += elementSize.Direct;
                    lineSize.Indirect = Math.Max(lineSize.Indirect, elementSize.Indirect);
                }
            }

            totalSize.Direct = Math.Max(lineSize.Direct, totalSize.Direct);
            totalSize.Indirect += lineSize.Indirect;

            return new Size(totalSize.Width, totalSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Orientation o = Orientation;
            OrientedSize lineSize = new OrientedSize(o);
            OrientedSize maximumSize = new OrientedSize(o, finalSize.Width, finalSize.Height);

            double itemWidth = ItemWidth;
            double itemHeight = ItemHeight;
            bool hasFixedWidth = !itemWidth.IsNaN();
            bool hasFixedHeight = !itemHeight.IsNaN();
            double indirectOffset = 0;
            double? directDelta = (o == Orientation.Horizontal) ?
                (hasFixedWidth ? (double?)itemWidth : null) :
                (hasFixedHeight ? (double?)itemHeight : null);

            UIElementCollection children = Children;
            int count = children.Count;
            int lineStart = 0;
            for (int lineEnd = 0; lineEnd < count; lineEnd++)
            {
                UIElement element = children[lineEnd];

                OrientedSize elementSize = new OrientedSize(
                    o,
                    hasFixedWidth ? itemWidth : element.DesiredSize.Width,
                    hasFixedHeight ? itemHeight : element.DesiredSize.Height);

                if (NumericExtensions.IsGreaterThan(lineSize.Direct + elementSize.Direct, maximumSize.Direct))
                {
                    ArrangeLine(lineStart, lineEnd, directDelta, indirectOffset, lineSize.Indirect);

                    indirectOffset += lineSize.Indirect;
                    lineSize = elementSize;

                    if (NumericExtensions.IsGreaterThan(elementSize.Direct, maximumSize.Direct))
                    {
                        ArrangeLine(lineEnd, ++lineEnd, directDelta, indirectOffset, elementSize.Indirect);

                        indirectOffset += lineSize.Indirect;
                        lineSize = new OrientedSize(o);
                    }

                    lineStart = lineEnd;
                }
                else
                {
                    lineSize.Direct += elementSize.Direct;
                    lineSize.Indirect = Math.Max(lineSize.Indirect, elementSize.Indirect);
                }
            }

            if (lineStart < count)
            {
                ArrangeLine(lineStart, count, directDelta, indirectOffset, lineSize.Indirect);
            }

            return finalSize;
        }

        private void ArrangeLine(int lineStart, int lineEnd, double? directDelta, double indirectOffset, double indirectGrowth)
        {
            double directOffset = 0.0;

            Orientation o = Orientation;
            bool isHorizontal = o == Orientation.Horizontal;

            UIElementCollection children = Children;
            for (int index = lineStart; index < lineEnd; index++)
            {
                UIElement element = children[index];
                OrientedSize elementSize = new OrientedSize(o, element.DesiredSize.Width, element.DesiredSize.Height);

                double directGrowth = directDelta != null ?
                    directDelta.Value :
                    elementSize.Direct;

                Rect bounds = isHorizontal ?
                    new Rect(directOffset, indirectOffset, directGrowth, indirectGrowth) :
                    new Rect(indirectOffset, directOffset, indirectGrowth, directGrowth);
                element.Arrange(bounds);

                directOffset += directGrowth;
            }
        }
    }

    internal static partial class TypeConverters
    {
        internal static bool CanConvertTo<T>(Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            return (destinationType == typeof(string)) ||
                destinationType.IsAssignableFrom(typeof(T));
        }

        internal static object ConvertTo(TypeConverter converter, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }

            if (value == null && !destinationType.IsValueType)
            {
                return null;
            }
            else if (value != null && destinationType.IsAssignableFrom(value.GetType()))
            {
                return value;
            }
            throw new NotSupportedException();
        }
    }

    public partial class LengthConverter : TypeConverter
    {
        private static Dictionary<string, double> UnitToPixelConversions = new Dictionary<string, double>
        {
            { "px", 1.0 },
            { "in", 96.0 },
            { "cm", 37.795275590551178 },
            { "pt", 1.3333333333333333 }
        };

        public LengthConverter()
        {
        }

        public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
        {
            switch (Type.GetTypeCode(sourceType))
            {
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.String:
                    return true;
                default:
                    return false;
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
        {
            if (source == null)
            {
                throw new NotSupportedException();
            }

            string text = source as string;
            if (text != null)
            {
                if (string.Compare(text, "Auto", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return double.NaN;
                }

                string number = text;
                double conversionFactor = 1.0;
                foreach (KeyValuePair<string, double> conversion in UnitToPixelConversions)
                {
                    if (number.EndsWith(conversion.Key, StringComparison.Ordinal))
                    {
                        conversionFactor = conversion.Value;
                        number = text.Substring(0, number.Length - conversion.Key.Length);
                        break;
                    }
                }

                try
                {
                    return conversionFactor * Convert.ToDouble(number, cultureInfo);
                }
                catch (FormatException)
                {
                    throw new FormatException();
                }
            }

            return Convert.ToDouble(source, cultureInfo);
        }

        public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
        {
            return TypeConverters.CanConvertTo<double>(destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
        {
            if (value is double)
            {
                double length = (double)value;
                if (destinationType == typeof(string))
                {
                    return length.IsNaN() ?
                        "Auto" :
                        Convert.ToString(length, cultureInfo);
                }
            }

            return TypeConverters.ConvertTo(this, value, destinationType);
        }
    }

    internal static class NumericExtensions
    {
        public static bool IsNaN(this double value)
        {
            NanUnion union = new NanUnion { FloatingValue = value };

            ulong exponent = union.IntegerValue & 0xfff0000000000000L;
            if ((exponent != 0x7ff0000000000000L) && (exponent != 0xfff0000000000000L))
            {
                return false;
            }
            ulong mantissa = union.IntegerValue & 0x000fffffffffffffL;
            return mantissa != 0L;
        }

        public static bool IsGreaterThan(double left, double right)
        {
            return (left > right) && !AreClose(left, right);
        }

        public static bool AreClose(double left, double right)
        {
            if (left == right)
            {
                return true;
            }

            double a = (Math.Abs(left) + Math.Abs(right) + 10.0) * 2.2204460492503131E-16;
            double b = left - right;
            return (-a < b) && (a > b);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct NanUnion
        {
            [FieldOffset(0)]
            internal double FloatingValue;

            [FieldOffset(0)]
            internal ulong IntegerValue;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct OrientedSize
    {
        private Orientation _orientation;

        public Orientation Orientation
        {
            get { return _orientation; }
        }

        private double _direct;

        public double Direct
        {
            get { return _direct; }
            set { _direct = value; }
        }

        private double _indirect;

        public double Indirect
        {
            get { return _indirect; }
            set { _indirect = value; }
        }

        public double Width
        {
            get
            {
                return (Orientation == Orientation.Horizontal) ?
                    Direct :
                    Indirect;
            }
            set
            {
                if (Orientation == Orientation.Horizontal)
                {
                    Direct = value;
                }
                else
                {
                    Indirect = value;
                }
            }
        }

        public double Height
        {
            get
            {
                return (Orientation != Orientation.Horizontal) ?
                    Direct :
                    Indirect;
            }
            set
            {
                if (Orientation != Orientation.Horizontal)
                {
                    Direct = value;
                }
                else
                {
                    Indirect = value;
                }
            }
        }

        public OrientedSize(Orientation orientation) :
            this(orientation, 0.0, 0.0)
        {
        }

        public OrientedSize(Orientation orientation, double width, double height)
        {
            _orientation = orientation;

            _direct = 0.0;
            _indirect = 0.0;

            Width = width;
            Height = height;
        }
    }
}

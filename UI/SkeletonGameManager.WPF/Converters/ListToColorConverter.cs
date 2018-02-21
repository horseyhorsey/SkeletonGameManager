using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SkeletonGameManager.WPF.Converters
{
    public class ListToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is List<byte> colorVals)
                return new Color() { R = colorVals[0], G = colorVals[1], B = colorVals[2], A=255 };

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;

            return new List<byte>() { color.R, color.G, color.B };
        }
    }
}

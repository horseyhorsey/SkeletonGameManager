using System;
using System.Globalization;
using System.Windows.Data;

namespace SkeletonGameManager.WPF.Converters
{
    public class DoubleToTimespanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;

            return TimeSpan.FromMilliseconds(val);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using SkeletonGame.Models;
using System;
using System.Windows;
using System.Windows.Data;

namespace SkeletonGameManager.Resources.Converters
{
    class StyleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FrameworkElement targetElement = values[0] as FrameworkElement;

            string styleName;
            if (values[1] != null)
            {
                styleName = values[1].ToString();

                if (styleName == null)
                    return null;
            }
            else
                return null;

            Style newStyle = (Style)targetElement.TryFindResource(styleName);

            if (newStyle == null)
                newStyle = (Style)targetElement.TryFindResource("GroupLayerSequenceStyle");

            return newStyle;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

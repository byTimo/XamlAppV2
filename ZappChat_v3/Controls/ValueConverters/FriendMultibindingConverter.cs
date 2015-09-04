using System;
using System.Globalization;
using System.Windows.Data;

namespace ZappChat_v3.Controls.ValueConverters
{
    public class FriendMultibindingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{values[0]} {values[1]}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
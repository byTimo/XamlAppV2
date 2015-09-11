using System;
using System.Globalization;
using System.Windows.Data;
using ZappChat_v3.Core.ChatElements.Enums;

namespace ZappChat_v3.Controls.ValueConverters
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (Status) value;
            return (int) status;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = (int) value;
            return (Status) index;
        }
    }
}
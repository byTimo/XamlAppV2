using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ZappChat_v3.Core.Messaging;

namespace ZappChat_v3.Controls.ValueConverters
{
    public class ListBoxItemHorizontalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var message = value as Message;
            return message.Type == MessageType.Outgoing ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
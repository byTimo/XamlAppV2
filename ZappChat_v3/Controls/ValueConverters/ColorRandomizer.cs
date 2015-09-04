using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Data;
using System.Windows.Media;

namespace ZappChat_v3.Controls.ValueConverters
{
    public class ColorRandomizer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rand = new Random();
            var red = rand.Next(0, 256);
            var green = rand.Next(0, 256);
            var blue = rand.Next(0, 256);
            Thread.Sleep(10);
            return new SolidColorBrush(Color.FromRgb((byte) red, (byte) green, (byte) blue));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
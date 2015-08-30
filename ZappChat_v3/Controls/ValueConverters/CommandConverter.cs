using System;
using System.Globalization;
using System.Windows.Data;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Controls.ValueConverters
{
    public class CommandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var commandName = value as string;
            if(commandName == null) throw new NullReferenceException("Ошибка при определении имени команды");
            return CommandManager.GetCommand(commandName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
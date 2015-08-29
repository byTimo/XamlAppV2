using System.Windows.Input;

namespace ZappChat_v3.Core.Managers
{
    public static class CommandManager
    {
        private static Command _groupSettingOpenCommand;

        static CommandManager()
        {
            _groupSettingOpenCommand = new Command();
        }

        public static Command GroupSettingOpenCommand => _groupSettingOpenCommand;
    }
}
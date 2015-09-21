using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace ZappChat_v3.Core.Managers
{
    public static class CommandManager
    {
        public static Command GroupCreateCommand { get; }
        public static Command GroupDeleteCommand { get; }
        public static Command AddFriendInGroupCommand { get; }
        public static Command AddFriendCommand { get; }
        public static Command DeleteFriendCommand { get; }

        public static OpenCommand GroupSettingOpenCommand { get; }
        public static OpenCommand GroupCreateOpenCommand { get; }
        public static OpenCommand FriendChatOpenCommand { get; }
        public static OpenCommand SettingOpenCommand { get; }

        static CommandManager()
        {
            GroupCreateCommand = new Command("GroupCreate");
            GroupDeleteCommand = new Command("GroupDelete");
            AddFriendInGroupCommand = new Command("AddFriendInGroup");
            AddFriendCommand = new Command("AddFriend");
            DeleteFriendCommand = new Command("DeleteFriend");

            GroupSettingOpenCommand = new OpenCommand("OpenGroupSetting");
            GroupCreateOpenCommand = new OpenCommand("OpenGroupCreate");
            FriendChatOpenCommand = new OpenCommand("OpenFriendChat");
            SettingOpenCommand = new OpenCommand("OpenSettings");
        }

        /// <summary>
        /// Возвращает объект команды по её имени.
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <returns>Команда с заданным имененем</returns>
        public static Command GetCommand(string commandName)
        {
            var proprtyName = string.Concat(commandName, "Command");
            var type = typeof (CommandManager);
            var typeProperties = type.GetProperties();
            var currentProperty = typeProperties.First(m => m.Name.Equals(proprtyName));
            return currentProperty.GetValue(null) as Command;
        }
    }
}
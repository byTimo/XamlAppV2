using System;
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
            switch (commandName)
            {
                case "OpenGroupSetting":
                    return GroupSettingOpenCommand;
                case "OpenGroupCreate":
                    return GroupCreateOpenCommand;
                case "OpenFriendChat":
                    return FriendChatOpenCommand;
                case "OpenSettings":
                    return SettingOpenCommand;
                case "GroupCreate":
                    return GroupCreateCommand;
                case "GroupDelete":
                    return GroupDeleteCommand;
                case "AddFriendInGroup":
                    return AddFriendInGroupCommand;
                case "AddFriend":
                    return AddFriendCommand;
                case "DeleteFriend":
                    return DeleteFriendCommand;
                default:
                    throw new ArgumentException("Неизвестное имя команды");
            }

        }
    }
}
using System;
using System.Windows.Input;

namespace ZappChat_v3.Core.Managers
{
    public static class CommandManager
    {
        public static event Action<string> PreviewExecuteCommand;
        public static event Action CloseCurrentContent;
        static CommandManager()
        {
            OpenGroupSettingCommand = new Command("OpenGroupSetting", OnPreviewExecuteCommand);
            OpenGroupCreateCommand = new Command("OpenGroupCreate", OnPreviewExecuteCommand);
            OpenFriendChatCommand = new Command("OpenFriendChat", OnPreviewExecuteCommand);
            GroupCreateCommand = new Command("GroupCreate", OnPreviewExecuteCommand);
            GroupDeleteCommand = new Command("GroupDelete", OnPreviewExecuteCommand);
            AddFriendInGroupCommand = new Command("AddFriendInGroup", OnPreviewExecuteCommand);
            AddFriendCommand = new Command("AddFriend", OnPreviewExecuteCommand);
            DeleteFriendCommand = new Command("DeleteFriend", OnPreviewExecuteCommand);
        }

        public static Command OpenGroupSettingCommand { get; }
        public static Command OpenGroupCreateCommand { get; }
        public static Command OpenFriendChatCommand { get; }
        public static Command GroupCreateCommand { get; }
        public static Command GroupDeleteCommand { get; }
        public static Command AddFriendInGroupCommand { get; }
        public static Command AddFriendCommand { get; }
        public static Command DeleteFriendCommand { get; }

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
                    return OpenGroupSettingCommand;
                case "OpenGroupCreate":
                    return OpenGroupCreateCommand;
                case "OpenFriendChat":
                    return OpenFriendChatCommand;
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

        private static void OnPreviewExecuteCommand(string obj)
        {
            PreviewExecuteCommand?.Invoke(obj);
        }

        public static void CloseContnt()
        {
            CloseCurrentContent?.Invoke();
        }
    }
}
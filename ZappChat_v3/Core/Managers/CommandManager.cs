using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace ZappChat_v3.Core.Managers
{
    public static class CommandManager
    {
        private static readonly Dictionary<string, Command> CommandDictionary = new Dictionary<string, Command>();

        static CommandManager()
        {
            RegisterNewOpenCommand("OpenGroupSetting", new OpenCommand("OpenGroupSetting"));
            RegisterNewOpenCommand("OpenGroupCreate", new OpenCommand("OpenGroupCreate"));
            RegisterNewOpenCommand("OpenFriendChat", new OpenCommand("OpenFriendChat"));
            RegisterNewOpenCommand("OpenSettings", new OpenCommand("OpenSettings"));

            RegisterNewCommand("GroupCreate", new Command("GroupCreate"));
            RegisterNewCommand("GroupDelete", new Command("GroupDelete"));
            RegisterNewCommand("AddFriendInGroup", new Command("AddFriendInGroup"));
            RegisterNewCommand("AddFriend", new Command("AddFriend"));
            RegisterNewCommand("DeleteFriend", new Command("DeleteFriend"));
        }

        /// <summary>
        /// Возвращает объект команды по её имени.
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <returns>Команда с заданным имененем</returns>
        public static Command GetCommand(string commandName)
        {
            if (!CommandDictionary.ContainsKey(commandName))
                throw new NullReferenceException($"Команды с имененм {commandName} нет");
            return CommandDictionary[commandName];
        }

        /// <summary>
        /// Возвращает объект открывающей команды по её имени.
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <returns>Открывающая команда с заданным именем</returns>
        public static OpenCommand GetOpenCommand(string commandName)
        {
            if (!CommandDictionary.ContainsKey(commandName))
                throw new NullReferenceException($"Команды с имененм {commandName} нет");
            var command = CommandDictionary[commandName];
            if (command.GetType() != typeof (OpenCommand)) return null;
            return command as OpenCommand;
        }

        /// <summary>
        /// Регистрация в мнеджере команд новой команды
        /// </summary>
        /// <param name="commandName">Имя новой команды</param>
        /// <param name="command">Объект класса Command, представляющий новую команду</param>
        public static void RegisterNewCommand(string commandName, Command command)
        {
            if (CommandDictionary.ContainsKey(commandName)) return;
            CommandDictionary[commandName] = command;
        }

        /// <summary>
        /// Регистрация в мнеджере команд новой открывающей команды
        /// </summary>
        /// <param name="commandName">Имя новой открывающей команды</param>
        /// <param name="openCommand">Объект класса OpenCommand, представляющий новую команду</param>
        public static void RegisterNewOpenCommand(string commandName, OpenCommand openCommand)
        {
            if (CommandDictionary.ContainsKey(commandName)) return;
            if (openCommand.GetType() != typeof (OpenCommand))
                throw new ArgumentException("Переданная команда не является OpenCommand");
            CommandDictionary[commandName] = openCommand;
        }
    }
}
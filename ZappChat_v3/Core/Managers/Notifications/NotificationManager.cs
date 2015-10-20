using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ZappChat_v3.Core.Managers.Notifications
{
    public static class NotificationManager
    {
        /// <summary>
        /// Возвращает коллекцию оповещений.
        /// </summary>
        public static ObservableCollection<Notification> Notifications { get; } = new ObservableCollection<Notification>();

        public static event Action<Notification> NewNotification;

        /// <summary>
        /// Добавить новое оповещение
        /// </summary>
        /// <param name="message">Сообщение оповещения</param>
        /// <param name="command">Команда, привязанная к оповещению</param>
        /// <param name="param">Параметр команды</param>
        public static void AddNewNotification(string message, NotificationType type, Command command = null, object param = null)
        {
            var newNotification = new Notification(Notifications.Count, message,type, command, param);
            Notifications.Add(newNotification);
            OnNewNotification(newNotification);
        }

        private static void OnNewNotification(Notification obj)
        {
            NewNotification?.Invoke(obj);
        }
    }
}
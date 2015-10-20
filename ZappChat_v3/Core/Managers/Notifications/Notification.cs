namespace ZappChat_v3.Core.Managers.Notifications
{
    public class Notification
    {
        public long Number { get; private set; }
        public string Message { get; private set; }
        public NotificationType Type { get; private set; }
        public Command Command { get; private set; }
        public object CommandParameter { get; private set; }

        public Notification(long number, string message, NotificationType type, Command command = null, object param = null)
        {
            Number = number;
            Message = message;
            Type = type;
            Command = command;
            CommandParameter = param;
        }
    }
}
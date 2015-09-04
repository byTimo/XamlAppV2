namespace ZappChat_v3.Core.Messaging
{
    public enum MessageClass
    {
        /// <summary>
        /// Системное сообщение
        /// </summary>
        System,

        /// <summary>
        /// Сообщение с текстом
        /// </summary>
        Text,

        /// <summary>
        /// Сообщение о передаче файла
        /// </summary>
        File,

        /// <summary>
        /// Стикер
        /// </summary>
        Sticker
    }
}
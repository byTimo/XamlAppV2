namespace ZappChat_v3.Core.Managers.WebSocket
{
    public enum WebSocketStatus
    {
        /// <summary>
        /// Приложение отключено от сервера автоматически. Этот статус появляется, если приложение ещё не подключалось либо пропал сетевой сигнал.
        /// </summary>
        Disconnected,
        /// <summary>
        /// Приложение подключено к серверу
        /// </summary>
        Connected,
        /// <summary>
        /// Приложение отключено от сервера вручную.
        /// </summary>
        UserDisconnected
    }
}
namespace ZappChat_v3.Core.Managers
{
    /// <summary>
    /// Перечисление флагов, контролирующих сессию между подключёнными пирами
    /// </summary>
    public enum CallControlFlag
    {
        /// <summary>
        /// Ошибка TODO
        /// </summary>
        Error = 0,
        /// <summary>
        /// Запрос к серверу на определение IP-адреса пира
        /// </summary>
        IpMemberDiscovery = 1,
        /// <summary>
        /// Ответ сервера с IP-адресом пира или с null
        /// </summary>
        IpMemberOffer = 2,
        /// <summary>
        /// Запрос к пиру, означающий новое подключение
        /// </summary>
        HelloQuestion = 3,
        /// <summary>
        /// Ответ от пира, к которому пошёл запрос
        /// </summary>
        HelloAnswer = 4,
        /// <summary>
        /// Запрос на создание голосовой сессии
        /// </summary>
        CreateSession = 5,
        /// <summary>
        /// Одобрение голосовой сессии
        /// </summary>
        ApproveSession = 6,
        /// <summary>
        /// Отказ от голосовой сессии
        /// </summary>
        RefuseSession = 7,
        /// <summary>
        /// Переданные данные
        /// </summary>
        DataTransfer = 8,
        /// <summary>
        /// Завершение голосовой сессии
        /// </summary>
        ByeQuestion = 9,
        /// <summary>
        /// Подтверждение завершения голосовой сессии
        /// </summary>
        ByeAnswer = 10
    }
}
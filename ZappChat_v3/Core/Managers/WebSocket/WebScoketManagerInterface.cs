using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ZappChat_v3.Core.Managers.WebSocket
{
    public static partial class WebSocketManager
    {
        private static bool _tryConnected;
        /// <summary>
        /// Метод начинает подключение к серверу Запчата
        /// </summary>
        public static void Connect()
        {
            if (Status == WebSocketStatus.Connected) return;
            EventInvoker(Connecting);
            //-- таймер ---
            WebSocket.Open();
            _tryConnected = true;
            while (_tryConnected)
            {
                Thread.Sleep(1000);
            }
            return;
        }
        /// <summary>
        /// Метод отключает приложение от сервера
        /// </summary>
        public static void Disconnect()
        {
            if (Status != WebSocketStatus.Connected) return;
            EventInvoker(Disconnecting);
            WebSocket.Close();
        }
//@TODO сделать какой нибудь класс описывающий результат отправки: отправлено, ошибка из-за и т.п
        public static void Send(Dictionary<string, object> sendingObject)
        {
            if (OutgoingObjectQueue.Count >= 3) return;
            var jObject = JObject.FromObject(sendingObject);
            OutgoingObjectQueue.Enqueue(jObject);
            lock (ThreadBlocker)
            {
                if (!SendOutgoingObjectTimer.IsEnabled)
                    ManuallySendObject();
            }
        }
    }
}
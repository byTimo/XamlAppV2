using System;
using System.Collections.Generic;
using System.Net;
using Lidgren.Network;

namespace ZappChat_v3.Core.Managers
{
    public static class P2PManager
    {
        private static NetPeer _peer;
        private static NetPeerConfiguration _configuration;
        private static Action<int,string, byte[], NetConnection> _soundCallback;

        private static NetPeerConfiguration Config
        {
            get
            {
                if (_configuration != null) return _configuration;
                _configuration = new NetPeerConfiguration("ZappChatPeer")
                {
                    AcceptIncomingConnections = true
                };
                _configuration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
                _configuration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
                return _configuration;
            }
        }
        private static NetPeer Peer
        {
            get
            {
                if (_peer != null) return _peer;
                _peer = new NetPeer(Config);
                _peer.RegisterReceivedCallback(GotDataArray);
                return _peer;
            }
        }

        /// <summary>
        /// Происходит после успешного подключения удалённой точки
        /// </summary>
        public static event EventHandler<NetConnection> RemoteConnect;
        /// <summary>
        /// Происохдит, когда удалённая точка отключилась
        /// </summary>
        public static event EventHandler<NetConnection> RemoteDisconnect;
        /// <summary>
        /// Регистрация метода вызывающего класса. Данный метод будет вызван, когда будет получена информация с другого пира.
        /// </summary>
        /// <param name="callback">Метод вызвающего класса</param>
        public static void RegisterSoundCallBack(Action<int,string, byte[], NetConnection> callback)
        {
            _soundCallback = callback;
            Peer.Start();
            Support.Logger.Info("Peer is started. Port:{0}, Id:{1}", Peer.Port, NetUtility.ToHexString(Peer.UniqueIdentifier));
        }
        /// <summary>
        /// Присоединиться к пиру с заданной конечной точкой
        /// </summary>
        /// <param name="endPoint">Конечная точка удалённого пира</param>
        /// <returns>Подключение к заданному пиру</returns>
        public static NetConnection Connect(IPEndPoint endPoint)
        {
            Support.Logger.Trace("Create new connection to {0}:{1}", endPoint.Address, endPoint.Port);
            try
            {
                return Peer.Connect(endPoint);
            }
            catch (Exception e)
            {
                Support.Logger.Error(e, "Connection error: Connection to {0}:{1} is failed", endPoint.Address,
                    endPoint.Port);
                throw;
            }
        }
        /// <summary>
        /// Отключает удалённый пир
        /// </summary>
        /// <param name="connection">Подключение к пиру</param>
        /// <param name="peerId">Прощальное сообщение пиру</param>
        public static void Disconnect(NetConnection connection, string peerId)
        {
            connection.Disconnect(peerId);
        }
        /// <summary>
        /// Отправить управляющий флаг одному подключению
        /// </summary>
        /// <param name="connection">Целевое подлючение</param>
        /// <param name="controlFlag">Флаг</param>
        public static void SendControlFlag(NetConnection connection, int controlFlag, string peerId)
        {
            var connecionList = new List<NetConnection> {connection};
            Send(connecionList, controlFlag, peerId, null);
        }
        /// <summary>
        /// Отправить управляющий флаги для сеанса
        /// </summary>
        /// <param name="connections">Целевые подключения</param>
        /// <param name="controlFlag">Флаг</param>
        public static void SendControlFlag(List<NetConnection> connections, int controlFlag, string peerId)
        {
            Send(connections, controlFlag, peerId, null);
        }
        /// <summary>
        /// Отправить массив байт одному подключению
        /// </summary>
        /// <param name="connection">Целевое подключение</param>
        /// <param name="controlFlag">Флаг</param>
        /// <param name="data">Массив отправляющихся байт</param>
        public static void SendData(NetConnection connection, int controlFlag, string peerId, byte[] data)
        {
            var connectioList = new List<NetConnection> {connection};
            Send(connectioList, controlFlag, peerId, data);
        }
        /// <summary>
        /// Отправить массив байт
        /// </summary>
        /// <param name="connections">Целевые подключения</param>
        /// <param name="controlFlag">Флаг</param>
        /// <param name="data">Массив отправляющихся байт</param>
        public static void SendData(List<NetConnection> connections, int controlFlag, string peerId, byte[] data)
        {
            Send(connections, controlFlag, peerId, data);
        }
        private static void Send(List<NetConnection> connections, int controlFlag, string peerId, byte[] data)
        {
            var message = Peer.CreateMessage();
            message.Write(controlFlag);
            message.Write(peerId);
            if (data != null)
            {
                message.WritePadBits();
                message.Write(data);
            }
            var method = data == null ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.Unreliable;
            Peer.SendMessage(message, connections, method , 0);
            Support.Logger.Trace("Send message {0} data. ControlFlag: {1}", data != null ? "with" : "without",
                controlFlag);
        }
        private static void GotDataArray(object gotterPeer)
        {
            NetIncomingMessage im;
            var currentPeer = ((NetPeer) gotterPeer);
            while ((im = currentPeer.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        var debugInfo = im.ReadString();
                        Support.Logger.Trace("P2PManager.DebugMessage: {0}", debugInfo);
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        var errorInfo = im.ReadString();
                        Support.Logger.Error("P2PManager.ErrorMessage: {0}", errorInfo);
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        Support.Logger.Trace("Discovery request from {0}", im.SenderEndPoint.ToString());
                        currentPeer.SendDiscoveryResponse(null, im.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        Support.Logger.Trace("Discovery response from {0}", im.SenderEndPoint.ToString());
                        currentPeer.Connect(im.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        CheckConnectionStatus(im);
                        break;
                    case NetIncomingMessageType.Data:
                        CheckData(im);
                        break;
                    default:
                        Support.Logger.Warn("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
                        break;
                }
                currentPeer.Recycle(im);
                
            }
        }

        private static void CheckConnectionStatus(NetIncomingMessage im)
        {
            var currentStatus = (NetConnectionStatus) im.ReadByte();
            var sender = NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier);
            var reason = im.ReadString();
            switch (currentStatus)
            {
                case NetConnectionStatus.Connected:
                    Support.Logger.Trace("{0} is connected. Reason: {1}", sender, reason);
                    OnRemoteConnect(im.SenderConnection);
                    break;
                case NetConnectionStatus.Disconnected:
                    Support.Logger.Trace("{0} is disconnected. Reason: {1}", sender, reason);
                    OnRemoteDisconnect(im.SenderConnection);
                    break;
                //@TODO сообщение о смене статуса, вызавать кучу событий, всё залогировать
                default:
                    Support.Logger.Warn("Unhandled type of NetConnection status: {0}", currentStatus);
                    break;
            }
        }

        private static void CheckData(NetIncomingMessage im)
        {
            var controlFlag = im.ReadInt32();
            var peerId = im.ReadString();
            byte[] buffer = null;
//@TODO пока через try/catch, потом можно на конкретные значения привязать
            try
            {
                var bufferSize = im.ReadInt32();
                im.SkipPadBits();
                buffer = im.ReadBytes(bufferSize);
            }
            finally
            {
                Support.Logger.Trace("Got message. ControlFlag: {0}{1}", controlFlag,
                    buffer != null ? ", bufferSize: " + buffer.Length : "");
                _soundCallback.Invoke(controlFlag, peerId, buffer, im.SenderConnection);
            }
        }
#region Event invokers
        private static void OnRemoteConnect(NetConnection e)
        {
            var handler = RemoteConnect;
            handler?.Invoke(null, e);
        }
        private static void OnRemoteDisconnect(NetConnection e)
        {
            var handler = RemoteDisconnect;
            handler?.Invoke(null, e);
        }
#endregion
    }
}
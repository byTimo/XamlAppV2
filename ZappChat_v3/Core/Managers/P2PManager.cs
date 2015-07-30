using System;
using System.Collections.Generic;
using System.Web;
using Lidgren.Network;

namespace ZappChat_v3.Core.Managers
{
    public static class P2PManager
    {
        private static NetPeer peer;
        private static NetPeerConfiguration configuration;

        private static NetPeerConfiguration Config
        {
            get
            {
                if (configuration != null) return configuration;
                configuration = new NetPeerConfiguration("ZappChatPeer");
                configuration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
                configuration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
//@TODO - пока не нужно, но может понадобиться
//                configuration.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
//@TODO - если понадобится при определении пира подключения, но пока попробуем так
//                configuration.EnableMessageType(NetIncomingMessageType.UnconnectedData);
                return configuration;
            }
        }
        private static NetPeer Peer
        {
            get
            {
                if (peer != null) return peer;
                peer = new NetPeer(Config);
                peer.RegisterReceivedCallback(GotDataArray);
                peer.Start();
                return peer;
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
        /// Отправить управляющий флаги для сеанса
        /// </summary>
        /// <param name="connections">Целевые подключения</param>
        /// <param name="controlFlag">Флаг</param>
        public static void SendControlFlag(List<NetConnection> connections, int controlFlag)
        {
            Send(connections, controlFlag, null);
        }
        /// <summary>
        /// Отправить звуковые байты
        /// </summary>
        /// <param name="connections">Целевые подключения</param>
        /// <param name="controlFlag">Флаг, указывающий на сообщение со звуком</param>
        /// <param name="data">Байты кодированного звука</param>
        public static void SendSound(List<NetConnection> connections, int controlFlag, byte[] data)
        {
            Send(connections, controlFlag, data);
        }

        private static void Send(List<NetConnection> connections, int controlFlag, byte[] data)
        {
            var message = Peer.CreateMessage();
            message.Write(controlFlag);
            if (data != null) message.Write(data);
            var method = data == null ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.Unreliable;
            Peer.SendMessage(message, connections, method , 0);
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
                        Peer.SendDiscoveryResponse(null, im.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        Support.Logger.Trace("Discovery response from {0}", im.SenderEndPoint.ToString());
                        Peer.Connect(im.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        CheckConnectionStatus(im);
                        break;
                    case NetIncomingMessageType.Data:
                        //@TODO обработать данные PeripheryManager-ом
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
#region Event invokers

        private static void OnRemoteConnect(NetConnection e)
        {
            var handler = RemoteConnect;
            if (handler != null) handler(null, e);
        }
        private static void OnRemoteDisconnect(NetConnection e)
        {
            var handler = RemoteDisconnect;
            if (handler != null) handler(null, e);
        }
#endregion
    }
}
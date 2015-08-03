using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Lidgren.Network;
using NAudio.Wave;
using ZappChat_v3.Core.ChatElements;

namespace ZappChat_v3.Core.Managers
{
    public static partial class CallManager
    {
        private static Dictionary<ChatMember, NetConnection> _connections;
        private static NetConnection _serverConnection;
        private static Dictionary<ChatMember, NetConnection> CurrentConnections
        {
            get
            {
                if (_connections != null) return _connections;
                _connections = new Dictionary<ChatMember, NetConnection>();
                return _connections;
            }
        }

        private static NetConnection ConnectionWithServer
        {
            get
            {
                if (_serverConnection != null) return _serverConnection;
                var serverEndPoint = new IPEndPoint(Constants.ServerIp, Constants.ServerPort);
                _serverConnection = P2PManager.Connect(serverEndPoint);
                return _serverConnection;
            }
        }
        /// <summary>
        /// Начать звонок с объектом чата
        /// </summary>
        /// <param name="chatMember">Объект чата</param>
        public static void BeginCallWithChatMember(ChatMember chatMember)
        {
            var bytes = Encoding.UTF8.GetBytes(chatMember.ChatMemberId);
            P2PManager.SendData(ConnectionWithServer, (int)CallControlFlag.IpMemberDiscovery, _peerId, bytes);
            OnBeginCall(new CallEventArgs(chatMember, false));
        }

        private static void SendPeripheryData(object sender, WaveInEventArgs e)
        {
            P2PManager.SendData(CurrentCall.Value, (int)CallControlFlag.DataTransfer, _peerId, e.Buffer);
        }
    }
}
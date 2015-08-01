using System;
using System.Collections.Generic;
using System.Net;
using Lidgren.Network;
using ZappChat_v3.Core.ChatElements;

namespace ZappChat_v3.Core.Managers
{
    public static class CallManager
    {
        private static Dictionary<ChatMember, NetConnection> connections;
        private static NetConnection serverConnection;
        private static Dictionary<ChatMember, NetConnection> CurrentConnections
        {
            get
            {
                if (connections != null) return connections;
                connections = new Dictionary<ChatMember, NetConnection>();
                return connections;
            }
        }

        private static NetConnection ConnectionWithServer
        {
            get
            {
                if (serverConnection != null) return serverConnection;
                var serverEndPoint = new IPEndPoint(Constants.ServerIp, Constants.ServerPort);
                serverConnection = P2PManager.Connect(serverEndPoint);
                return serverConnection;
            }
        }

        /// <summary>
        /// Начать звонок с объектом чата
        /// </summary>
        /// <param name="chatMember">Объект чата</param>
        public static void BeginCallWithChatMember(ChatMember chatMember)
        {
            throw new NotImplementedException();
        }


        
    }
}
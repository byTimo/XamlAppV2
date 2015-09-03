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
        private static Dictionary<IChatMember, NetConnection> _connections;
        private static NetConnection _serverConnection;
        private static Dictionary<IChatMember, NetConnection> CurrentConnections
        {
            get
            {
                if (_connections != null) return _connections;
                _connections = new Dictionary<IChatMember, NetConnection>();
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
        public static void CallChatMamber(IChatMember chatMember)
        {
            CurrentConnections.Add(chatMember, null);
            var bytes = Encoding.UTF8.GetBytes(chatMember.ChatMemberId);
            P2PManager.SendData(ConnectionWithServer, (int)CallControlFlag.IpMemberDiscovery, _peerId, bytes);
            OnBeginCall(new CallEventArgs(chatMember, false));
        }
        /// <summary>
        /// Ответить или отменить звонок 
        /// </summary>
        /// <param name="chatMamber">Объект чата</param>
        /// <param name="answer">Отвечаем на звонок</param>
        public static void AnswerChatMamber(IChatMember chatMamber, bool answer)
        {
            var mamberAndConnection = CurrentConnections.FirstOrDefault(c => c.Key.Equals(chatMamber));
            if (mamberAndConnection.Value == null)
            {
                var ex = new NullReferenceException("CallManager.AnswerChatMamaber null connection");
                Support.Logger.Fatal(ex);
                throw ex;
            }
            if (!answer)
            {
                P2PManager.SendControlFlag(mamberAndConnection.Value, (int)CallControlFlag.RefuseSession, _peerId);
            }
            else
            {
                P2PManager.SendControlFlag(mamberAndConnection.Value, (int)CallControlFlag.ApproveSession, _peerId);
                CurrentCall = mamberAndConnection;
            }
        }
        /// <summary>
        /// Положить трубку
        /// </summary>
        public static void HangUp()
        {
            CurrentCall = new KeyValuePair<IChatMember, NetConnection>();
        }

        private static void SendPeripheryData(object sender, WaveInEventArgs e)
        {
            P2PManager.SendData(CurrentCall.Value, (int)CallControlFlag.DataTransfer, _peerId, e.Buffer);
        }
    }
}
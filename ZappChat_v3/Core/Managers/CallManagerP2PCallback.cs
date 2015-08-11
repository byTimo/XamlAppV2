using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Lidgren.Network;
using ZappChat_v3.Core.ChatElements;

namespace ZappChat_v3.Core.Managers
{
    public static partial class CallManager
    {
        //@TODO - после того, как узнаем свой id
        private static string _peerId = "1";
        private static KeyValuePair<ChatMember, NetConnection> _curentCall;
        private static Action<byte[], int, int> _playByteArrayAction;

        private static KeyValuePair<ChatMember, NetConnection> CurrentCall
        {
            get { return _curentCall; }
            set
            {
                PeripheryManager.StopTranslation();
                P2PManager.Disconnect(_curentCall.Value, _peerId);
                CurrentConnections.Remove(_curentCall.Key);
                _curentCall = value;
            }
        }

        static CallManager()
        {
            P2PManager.RegisterSoundCallBack(P2PCallback);
        }
        private static void P2PCallback(int flag, string peerId, byte[] data, NetConnection connection)
        {
            var controlFlag = (CallControlFlag)flag;
            switch (controlFlag)
            {
                case CallControlFlag.IpMemberDiscovery:
                    IpMemberDiscoveryHandler(connection);
                    break;
                case CallControlFlag.IpMemberOffer:
                    IpMemberOfferHandler(data);
                    break;
                case CallControlFlag.HelloQuestion:
                    HelloQuestionHandler(peerId, connection);
                    break;
                case CallControlFlag.HelloAnswer:
                    HelloAnswerHandler(connection);
                    break;
                case CallControlFlag.CreateSession:
                    CreateSessionHandler(peerId);
                    break;
                case CallControlFlag.ApproveSession:
                    ApproveSessionHandler(peerId, connection);
                    break;
                case CallControlFlag.RefuseSession:
                    RefuseSessionHandler(peerId, connection);
                    break;
                case CallControlFlag.DataTransfer:
                    DataTransferHandler(data, peerId, connection);
                    break;
                case CallControlFlag.ByeQuestion:
                    ByeQuestionHandler(peerId);
                    break;
                case CallControlFlag.ByeAnswer:
                    ByeAnswerHandler();
                    break;
            }
        }

        private static void IpMemberDiscoveryHandler(NetConnection connection)
        {
            Support.Logger.Error(
                "CallManager.P2PCallBack Error: Received IpMemberDiscovery request from {0}:{1}",
                connection.RemoteEndPoint.Address, connection.RemoteEndPoint.Port);
            var answerData = Encoding.UTF8.GetBytes("This responce send server only!");
            P2PManager.SendData(connection, (int) CallControlFlag.Error, _peerId, answerData);
        }

        private static void IpMemberOfferHandler(byte[] data)
        {
            var peerInfo = Encoding.UTF8.GetString(data).Split('|');
            var chatMember = CurrentConnections.FirstOrDefault(c => c.Key.ChatMemberId == peerInfo[0]);
            var sereverReturnIp = true;
            var ipAddress = peerInfo[1] != "" ? IPAddress.Parse(peerInfo[1]) : null;
            if (ipAddress == null)
            {
                Support.Logger.Warn("CallManager.P2PCallBack: Server return empty IP-address");
                sereverReturnIp = false;
                CurrentConnections.Remove(chatMember.Key);
            }
            else
            {
                var port = int.Parse(peerInfo[2]);
                Support.Logger.Trace("CallManager.P2PCallBack: Get IpAddress {0} - {1}:{2}", chatMember,
                    ipAddress, port);
                var endPoint = new IPEndPoint(ipAddress, port);
                var connection = P2PManager.Connect(endPoint);
                CurrentConnections[chatMember.Key] = connection;
                CurrentCall = chatMember;
                //@TODO - timer
                P2PManager.SendControlFlag(connection,(int)CallControlFlag.HelloQuestion, _peerId);
            }
            OnIpAddressOffer(new CallEventArgs(chatMember.Key, sereverReturnIp));
        }

        private static void HelloQuestionHandler(string peerId, NetConnection connection)
        {
//@TODO После реализации контент менеджера нужно определять конкретный ChatMember по id
            Support.Logger.Trace("CallManager.P2PCallback: Peer {0}-{1}:{2} connected", peerId,
                connection.RemoteEndPoint.Address, connection.RemoteEndPoint.Port);



            throw new NotImplementedException();
        }

        private static void HelloAnswerHandler(NetConnection connection)
        {
            Support.Logger.Trace("CallManager.P2PCallback: Peer {0}-{1}:{2} connected", _curentCall.Key,
                connection.RemoteEndPoint.Address, connection.RemoteEndPoint.Port);
            OnPeerConnect(new CallEventArgs(_curentCall.Key, true));
            //@TODO - timer
            P2PManager.SendControlFlag(connection, (int)CallControlFlag.CreateSession, _peerId);
        }

        private static void CreateSessionHandler(string peerId)
        {
            var chatMamber = CurrentConnections.FirstOrDefault(c => c.Key.ChatMemberId == peerId).Key;
            if (chatMamber == null)
            {
                Support.Logger.Error(
                    "CallManager.P2PCallback: CurrentConnections don't containt chatMamber with id:{0}", peerId);
                return;
            }
            Support.Logger.Trace("CalManager.P2PCallback: ChatMamber {0} create session");
            OnRemotePeerCall(new CallEventArgs(chatMamber, true));
        }

        private static void ApproveSessionHandler(string peerId, NetConnection connection)
        {
            if (CurrentCall.Key.ChatMemberId != peerId)
            {
                Support.Logger.Warn("CallManager.P2PCallback: Ignore session approve from {0} - {1}:{2}", peerId,
                    connection.RemoteEndPoint.Address, connection.RemoteEndPoint.Port);
                return;
            }
            Support.Logger.Trace("CallManager.P2PCallback: Approve session with {0}", CurrentCall);
            _playByteArrayAction = PeripheryManager.BindingTranslation(SendPeripheryData);
            PeripheryManager.StartTranslation();
            OnPeerAnswer(new CallEventArgs(CurrentCall.Key, true));
        }
        private static void RefuseSessionHandler(string peerId, NetConnection connection)
        {
            if (CurrentCall.Key.ChatMemberId != peerId)
            {
                Support.Logger.Warn("CallManager.P2PCallback: Ignore session refuse from {0} - {1}:{2}", peerId,
                    connection.RemoteEndPoint.Address, connection.RemoteEndPoint.Port);
                return;
            }
            Support.Logger.Trace("CallManager.P2PCallback: Refuse session with {0}", CurrentCall);
            P2PManager.Disconnect(CurrentCall.Value, _peerId);
            OnPeerAnswer(new CallEventArgs(CurrentCall.Key, false));
            CurrentCall = new KeyValuePair<ChatMember, NetConnection>();
        }

        private static void DataTransferHandler(byte[] data, string peerId, NetConnection connection)
        {
            if (CurrentCall.Key.ChatMemberId != peerId)
            {
                Support.Logger.Warn("CallManager.P2PCallback: Ignore data from {0} - {1}:{2}", peerId,
                    connection.RemoteEndPoint.Address, connection.RemoteEndPoint.Port);
                return;
            }
            _playByteArrayAction.Invoke(data, 0, data.Length);
        }

        private static void ByeQuestionHandler(string peerId)
        {
            if (CurrentCall.Key.ChatMemberId == peerId)
            {
                P2PManager.SendControlFlag(CurrentCall.Value, (int)CallControlFlag.ByeAnswer, _peerId);
                OnPeerDrop(new CallEventArgs(CurrentCall.Key, true));
                CurrentCall = new KeyValuePair<ChatMember, NetConnection>();
            }
            else
            {
                var chatMamber = CurrentConnections.FirstOrDefault(c => c.Key.ChatMemberId == peerId);
                if (chatMamber.Value == null) return;
                P2PManager.SendControlFlag(chatMamber.Value, (int)CallControlFlag.ByeAnswer, _peerId);
                OnPeerDrop(new CallEventArgs(chatMamber.Key, true));
                P2PManager.Disconnect(chatMamber.Value, _peerId);
                CurrentConnections.Remove(chatMamber.Key);
            }
        }

        private static void ByeAnswerHandler()
        {
            //Всем насрать!
        }
    }
}
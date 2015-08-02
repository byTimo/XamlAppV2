using System;
using System.Linq;
using System.Net;
using System.Text;
using Lidgren.Network;

namespace ZappChat_v3.Core.Managers
{
    public static partial class CallManager
    {
        static CallManager()
        {
            P2PManager.RegisterSoundCallBack(P2PCallback);
        }
        private static void P2PCallback(int flag, byte[] data, NetConnection connection)
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
                    HelloQuestionHandler(data, connection);
                    break;
                    //@TODO го дальше

            }
        }

        private static void IpMemberDiscoveryHandler(NetConnection connection)
        {
            Support.Logger.Error(
                "CallManager.P2PCallBack Error: Received IpMemberDiscovery request from {0}:{1}",
                connection.RemoteEndPoint.Address, connection.RemoteEndPoint.Port);
            var answerData = Encoding.UTF8.GetBytes("This responce send server only!");
            P2PManager.SendData(connection, (int) CallControlFlag.Error, answerData);
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
                Support.Logger.Trace("CallManager.P2PCallBack: Get IpAddress {0} - {1}:{2}", chatMember.Key,
                    ipAddress, port);
                var endPoint = new IPEndPoint(ipAddress, port);
                CurrentConnections.Add(chatMember.Key, P2PManager.Connect(endPoint));
            }
            OnIpAddressOffer(new CallEventArgs(chatMember.Key, sereverReturnIp));
        }

        private static void HelloQuestionHandler(byte[] data, NetConnection connection)
        {
//@TODO После реализации контент менеджера нужно определять конкретный ChatMember по id
            var chatMamberId = Encoding.UTF8.GetString(data);
            Support.Logger.Trace("CallManager.P2PCallback: Peer {0}-{1}:{2} connected", chatMamberId,
                connection.RemoteEndPoint.Address, connection.RemoteEndPoint.Port);
            throw new NotImplementedException();
        }

        private static void HellAnswerHandler(byte[] data, NetConnection connection)
        {
            var chatMamberId = Encoding.UTF8.GetString(data);
            Support.Logger.Trace("CallManager.P2PCallback: Peer {0}-{1}:{2} connected", chatMamberId,
                connection.RemoteEndPoint.Address, connection.RemoteEndPoint.Port);
            var peerInfo = Encoding.UTF8.GetString(data).Split('|');
            var chatMember = CurrentConnections.FirstOrDefault(c => c.Key.ChatMemberId == peerInfo[0]);
            OnPeerConnect(new CallEventArgs(chatMember.Key, true));
            //@TODO Сделать так, чтобы тот, кому идёт звонок - выделялся

        }
    }
}
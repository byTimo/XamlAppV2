using System;
using ZappChat_v3.Core.ChatElements;

namespace ZappChat_v3.Core.Managers
{
    public static partial class CallManager
    {
        /// <summary>
        /// Происходит, когда клиент обращается к серверу за внешним IP пира
        /// </summary>
        public static event EventHandler<CallEventArgs> BeginCall;
        /// <summary>
        /// Происходит, когда сервер отвечает клиенту
        /// </summary>
        public static event EventHandler<CallEventArgs> IpAddressOffer;
        /// <summary>
        /// Просходит, когда пир откликается на запрос
        /// </summary>
        public static event EventHandler<CallEventArgs> PeerConnect;
        /// <summary>
        /// Происходит, когда пир отвечает на запрос звонка
        /// </summary>
        public static event EventHandler<CallEventArgs> PeerAnswer;
        /// <summary>
        /// Происходит, когда пир завершает сессию звонка
        /// </summary>
        public static event EventHandler<CallEventArgs> PeerDrop;

        private static void OnBeginCall(CallEventArgs e)
        {
            BeginCall?.Invoke(null, e);
        }

        private static void OnIpAddressOffer(CallEventArgs e)
        {
            IpAddressOffer?.Invoke(null, e);
        }

        private static void OnPeerConnect(CallEventArgs e)
        {
            PeerConnect?.Invoke(null, e);
        }

        private static void OnPeerAnswer(CallEventArgs e)
        {
            PeerAnswer?.Invoke(null, e);
        }

        private static void OnPeerDrop(CallEventArgs e)
        {
            PeerDrop?.Invoke(null, e);
        }
    }

    public class CallEventArgs : EventArgs
    {
        public ChatMember Member { get; private set; }
        public bool Answer { get; private set; }

        public CallEventArgs(ChatMember member, bool answer)
        {
            Member = member;
            Answer = answer;
        }
    }
}
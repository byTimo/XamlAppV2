using System;

namespace ZappChat_v3.Core.Managers.WebSocket
{
    public static partial class WebSocketManager
    {
        private static readonly object TimerControlBlocker = new object();
        private static void ManuallySendObject()
        {
            if (OutgoingObjectQueue.Count == 0)
            {
                Support.Logger.Trace("Invoke manually send first queue element, when queue's count equals 0");
                return;
            }
            
            var sendingObject = OutgoingObjectQueue.Peek().ToString();
            if (Status != WebSocketStatus.Connected)
            {
                Support.Logger.Trace("Try send object, when WebSocket status {0}", Status);
                if (!SendOutgoingObjectTimer.IsEnabled) SendOutgoingObjectTimer.Start();
                return;
            }
            WebSocket.Send(sendingObject);
            OutgoingObjectQueue.Dequeue();
            if (OutgoingObjectQueue.Count != 0)
                SendOutgoingObjectTimer.Start();
        }
        private static void AutoSendObjects(object sender, EventArgs eventArgs)
        {
            if (OutgoingObjectQueue.Count == 0)
            {
                Support.Logger.Trace("Invoke automatical send first queue element, when queue's count equals 0");
                SendOutgoingObjectTimer.Stop();
                return;
            }
            if (Status != WebSocketStatus.Connected) return;

            while (OutgoingObjectQueue.Count != 0)
            {
                var sendingObject = OutgoingObjectQueue.Peek().ToString();
                if (Status != WebSocketStatus.Connected) return;
                WebSocket.Send(sendingObject);
                OutgoingObjectQueue.Dequeue();
                if (Status != WebSocketStatus.Connected) return;
            }
            lock (TimerControlBlocker)
            {
                if (OutgoingObjectQueue.Count == 0)
                    SendOutgoingObjectTimer.Stop();
            }
        }

        private static void ManuallyParseObject()
        {
            if (IncomingObjectQueue.Count == 0)
            {
                Support.Logger.Trace("Invoke manually parse first queue element, when queue's count equals 0");
                return;
            }
            ReceivedJObject(IncomingObjectQueue.Dequeue());
            if(IncomingObjectQueue.Count != 0 && !ParseIncomingObjectQueue.IsEnabled)
                ParseIncomingObjectQueue.Start();
        }
        private static void AutoParseObjects(object sender, EventArgs eventArgs)
        {
            if (OutgoingObjectQueue.Count == 0)
            {
                Support.Logger.Trace("Invoke automatical parse first queue element, when queue's count equals 0");
                ParseIncomingObjectQueue.Stop();
                return;
            }
            while (IncomingObjectQueue.Count != 0)
            {
                ReceivedJObject(IncomingObjectQueue.Dequeue());
            }
            lock (TimerControlBlocker)
            {
                if(IncomingObjectQueue.Count == 0 && !ParseIncomingObjectQueue.IsEnabled)
                    ParseIncomingObjectQueue.Stop();
            }
        }
    }
}
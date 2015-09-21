using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperSocket.ClientEngine;
using WebSocket4Net;

namespace ZappChat_v3.Core.Managers.WebSocket
{
    public static partial class WebSocketManager
    {
        private static readonly object ChangeConnectionStatusBlocker = new object();

        public const string ZappChatWebSocketUri = "ws://zappchat.ru:8888";

        private static WebSocket4Net.WebSocket _webSocket;
        private static WebSocket4Net.WebSocket WebSocket => _webSocket ?? (_webSocket = new WebSocket4Net.WebSocket(ZappChatWebSocketUri));

        public static WebSocketStatus Status { get; private set; }

        private static readonly Queue<JObject> IncomingObjectQueue;
        private static readonly Queue<JObject> OutgoingObjectQueue;

        private static readonly DispatcherTimer SendOutgoingObjectTimer;
        private static readonly DispatcherTimer ParseIncomingObjectQueue;

        public static event Action Connecting;
        public static event Action Connected;
        public static event Action Disconnecting;
        public static event Action Disconnected;

        static WebSocketManager()
        {
            WebSocket.Opened += WebSocketConnected;
            WebSocket.Closed += WebSocketDisconnected;
            WebSocket.Error += WebSocketOnError;
            WebSocket.MessageReceived += GetObject;

            IncomingObjectQueue = new Queue<JObject>();
            OutgoingObjectQueue = new Queue<JObject>();

            SendOutgoingObjectTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(2)};
            SendOutgoingObjectTimer.Tick += AutoSendObjects;
            ParseIncomingObjectQueue = new DispatcherTimer {Interval = TimeSpan.FromSeconds(2)};
            ParseIncomingObjectQueue.Tick += AutoParseObjects;
        }

        private static void WebSocketConnected(object sender, EventArgs eventArgs)
        {
            lock (ChangeConnectionStatusBlocker)
            {
                //_tryConnected = false;
                Status = WebSocketStatus.Connected;
                EventInvoker(Connected);
                Support.Logger.Info("Successfull connection to server");
            }
        }

        private static void WebSocketDisconnected(object sender, EventArgs eventArgs)
        {
            lock (ChangeConnectionStatusBlocker)
            {
                Status = WebSocketStatus.Disconnected;
                EventInvoker(Disconnected);
                Support.Logger.Info("Disconnected server");
            }
        }

//@TODO ----- Переделать EventArgs для Disconnecting - добавить туда информацию о том, кто запросил дисконект система или пользователь

        private static void WebSocketOnError(object sender, ErrorEventArgs errorEventArgs)
        {
            lock (ChangeConnectionStatusBlocker)
            {
                EventInvoker(Disconnecting);
                Support.Logger.Error(errorEventArgs.Exception,"Error in WebSocket");
                WebSocket.Close();
            }
        }

        private static void GetObject(object sender, MessageReceivedEventArgs messageReceivedEventArgs)
        {
            var gettingObject = (JObject) JsonConvert.DeserializeObject(messageReceivedEventArgs.Message);
            IncomingObjectQueue.Enqueue(gettingObject);
            ManuallyParseObject();
        }

        private static void EventInvoker(Action currentEvent)
        {
            if (currentEvent != null)
                Application.Current.Dispatcher.Invoke(currentEvent);
        }
    }
}
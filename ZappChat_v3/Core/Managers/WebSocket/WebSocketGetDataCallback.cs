using System;
using System.Net;
using System.Windows;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;

namespace ZappChat_v3.Core.Managers.WebSocket
{
    public static partial class WebSocketManager
    {
        public static event Action<WebSocketManagerEventArgs, object> LoginAuthorizationRequest;
        public static event Action<WebSocketManagerEventArgs, object> TokenAuthorizationRequest;
        public static event Action<WebSocketManagerEventArgs, object> AddFriendRequest;

        private static void ReceivedJObject(JObject receivedJObject)
        {
            var action = (string)receivedJObject["action"];
            if (action == null)
            {
                Support.Logger.Error("Received object didn't content action field");
                return;
            }
            var status = (string)receivedJObject["status"] == "ok" ? SendObjectResult.Successfull : SendObjectResult.Fail;
            switch (action)
            {
                case "client/auth":
                    var args = new WebSocketManagerEventArgs(status);
                    if (status == SendObjectResult.Fail)
                    {
                        args.Reason = (string) receivedJObject["reason"];
                        EventInvoker(LoginAuthorizationRequest, args, null);
                        return;
                    }
                    var token = (string)receivedJObject["token"];
                    EventInvoker(LoginAuthorizationRequest, args, token);
                    break;
            }
        }

        private static void EventInvoker(Action<WebSocketManagerEventArgs, object> currentEvent, WebSocketManagerEventArgs args, object obj)
        {
            Application.Current.Dispatcher.Invoke(() => currentEvent?.Invoke(args, obj));
        }
    }
}
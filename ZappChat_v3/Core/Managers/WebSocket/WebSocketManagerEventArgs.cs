using System;

namespace ZappChat_v3.Core.Managers.WebSocket
{
    public class WebSocketManagerEventArgs : EventArgs
    {
        public SendObjectResult Result { get; }
        public string Reason { get; set; }

        public WebSocketManagerEventArgs(SendObjectResult result)
        {
            Result = result;
        }
    }
}
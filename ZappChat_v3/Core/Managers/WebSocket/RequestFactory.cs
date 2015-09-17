using System.Collections.Generic;

namespace ZappChat_v3.Core.Managers.WebSocket
{
    public static class RequestFactory
    {
        public static Dictionary<string, object> LoginAuthorize(string userName, string password)
        {
            return new Dictionary<string, object>
            {
                {"action", "client/auth"},
                {"email", userName},
                {"password", password}
            };
        }
    }
}
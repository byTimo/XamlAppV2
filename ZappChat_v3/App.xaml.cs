using System;
using System.Threading;
using System.Windows;
using ZappChat_v3.Core;
using ZappChat_v3.Core.Managers;
using ZappChat_v3.Core.Managers.WebSocket;
using ZappChat_v3.Windows;

namespace ZappChat_v3
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
//@TODO - тоже в оконо в другое перенести!! 
        public static string CurrentUserId { get; private set; }

        private static string CurrentUserLogin { get; set; }
        private static string CurrentUserPassword { get; set; }
        private static string CurrentUserToken { get; set; }

        private static TEST_authorize _authorize;
        public static TEST_authorize Authorize => _authorize ?? (_authorize = new TEST_authorize());

        private static ChatWindow _chatWindow;
        public static ChatWindow ChatWindow => _chatWindow ?? (_chatWindow = new ChatWindow());

        public static void AuthorizeResponse(string login, string password)
        {
            if(string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return;
            CurrentUserLogin = login;
            CurrentUserPassword = password;
            var request = RequestFactory.LoginAuthorize(CurrentUserLogin, CurrentUserPassword);
            WebSocketManager.Send(request);

            if(WebSocketManager.Status != WebSocketStatus.Connected)
                WebSocketManager.Connect();
            Authorize.Hide();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            WebSocketManager.LoginAuthorizationRequest +=WebSocketManagerOnLoginAuthorizationRequest;
            Authorize.Show();
            //CurrentUserLogin = "andreykaka@mail.ru";
            //WebSocketManagerOnLoginAuthorizationRequest(new WebSocketManagerEventArgs(SendObjectResult.Successfull), "");
        }

        private void WebSocketManagerOnLoginAuthorizationRequest(WebSocketManagerEventArgs args, object o)
        {
            if (args.Result == SendObjectResult.Fail)
            {
                Support.Logger.Error(args.Reason);
                Authorize.Show();
                return;
            }
            var token = (string)o;
            CurrentUserToken = token;
            FileManager.ProfileFolder = CurrentUserLogin;

            CurrentUserId = "11";
            ChatWindow.UserName = CurrentUserLogin;
            ChatWindow.LastUserName = "Кондратьев";

            ChatWindow.Show();
        }


        private void App_OnExit(object sender, ExitEventArgs e)
        {
            PeripheryManager.FinalizePeriphery();
        }
    }
}

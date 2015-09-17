using System.Threading;
using System.Windows;
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
        public static ChatWindow CurrentChatWindow { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var testUserLogin = "Andrey";
            FileManager.ProfileFolder = testUserLogin;
            CurrentChatWindow = new ChatWindow();
            CurrentChatWindow.Show();

            WebSocketManager.Connect();
            WebSocketManager.Connected += () => WebSocketManager.Send(RequestFactory.LoginAuthorize("andreykaka@mail.ru", "123456"));
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            PeripheryManager.FinalizePeriphery();
        }
    }
}

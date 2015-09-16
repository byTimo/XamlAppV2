using System.Windows;
using ZappChat_v3.Core.Managers;
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
            var testUserLogin = "Andey";
            FileManager.ProfileFolder = testUserLogin;
            CurrentChatWindow = new ChatWindow();
            CurrentChatWindow.Show();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            PeripheryManager.FinalizePeriphery();
        }
    }
}

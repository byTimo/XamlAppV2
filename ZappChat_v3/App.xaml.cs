using System.Windows;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            PeripheryManager.FinalizePeriphery();
        }
    }
}

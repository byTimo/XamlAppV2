using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZappChat_v3.Core.Managers.WebSocket;

namespace ZappChat_v3.Windows
{
    /// <summary>
    /// Interaction logic for TEST_authorize.xaml
    /// </summary>
    public partial class TEST_authorize : Window
    {
        private Action OnConnected;
        public TEST_authorize()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(LoginTextBox.Text)) return;
            if(string.IsNullOrWhiteSpace(PasswordTextBox.Text)) return;

            App.AuthorizeResponse(LoginTextBox.Text, PasswordTextBox.Text);
        }
    }
}

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ZappChat_v3.Annotations;
using CommandManager = ZappChat_v3.Core.Managers.CommandManager;

namespace ZappChat_v3.Windows
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow: Window, INotifyPropertyChanged
    {
        private readonly ChatWindowModel _model;
        public ChatWindow()
        {
            InitializeComponent();
            _model = new ChatWindowModel();
            DataContext = _model;
            CommandManager.PreviewExecuteCommand +=CommandManagerOnPreviewExecuteCommand;
        }

        private void CommandManagerOnPreviewExecuteCommand(string s)
        {
            StatusButton.IsChecked = false;
            SettingButton.IsChecked = false;
        }

        private void AppShutdown(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StatusButton_Checked(object sender, RoutedEventArgs e)
        {
            StatusMenu.SetGroup(_model.GroupCollection);
            FullBlockator.Visibility = Visibility.Visible;
            StatusMenu.Visibility = Visibility.Visible;
        }

        private void StatusButton_Unchecked(object sender, RoutedEventArgs e)
        {
            FullBlockator.Visibility = Visibility.Collapsed;
            StatusMenu.Visibility = Visibility.Collapsed;
        }

        private void WindowStateChange(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void HideWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void friendList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(friendList.SelectedIndex == -1) return;
            var friend = friendList.Items[friendList.SelectedIndex];
            CommandManager.OpenFriendChatCommand.DoExecute(friend);
            friendList.SelectedIndex = -1;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StatusButton.IsChecked = false;
        }

        private void friendList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void SettingButton_Checked(object sender, RoutedEventArgs e)
        {
            CommandManager.OpenSettingCommand.DoExecute(null);
        }

        private void SettingButton_Unchecked(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

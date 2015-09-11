using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ZappChat_v3.Annotations;
using ZappChat_v3.Core;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Controls
{
    [TemplatePart(Name = "ChatButton", Type = typeof(Button))]
    [TemplatePart(Name = "InfoButton", Type = typeof(Button))]
    [TemplatePart(Name = "DeleteButton", Type = typeof(Button))]
    [TemplatePart(Name = "YesButton", Type = typeof(Button))]
    [TemplatePart(Name = "NoButton", Type = typeof(Button))]
    [TemplatePart(Name = "DeleteStateGrid",Type = typeof(Grid))]
    public class FriendContextMenu : ContextMenu
    {
        static FriendContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (FriendContextMenu),
                new FrameworkPropertyMetadata(typeof (FriendContextMenu)));
        }
        private Button _chatButton;
        private Button _infoButton;
        private Button _deleteButton;
        private Button _yesButton;
        private Button _noButton;
        private Grid _deleteStateGrid;

        public override void OnApplyTemplate()
        {
            _chatButton = GetTemplateChild("ChatButton") as Button;
            _infoButton = GetTemplateChild("InfoButton") as Button;
            _deleteButton = GetTemplateChild("DeleteButton") as Button;
            _yesButton = GetTemplateChild("YesButton") as Button;
            _noButton = GetTemplateChild("NoButton") as Button;
            _deleteStateGrid = GetTemplateChild("DeleteStateGrid") as Grid;

            if (_chatButton == null || _infoButton == null || _deleteButton == null || _yesButton == null ||
                _noButton == null || _deleteStateGrid == null)
                throw new NullReferenceException("Не нашёл одну из кнопок!");
            _chatButton.Click += ChatButtonOnClick;
            _deleteButton.Click +=DeleteButtonOnClick;
            _yesButton.Click += YesButtonOnClick;
            _noButton.Click +=NoButtonOnClick;
        }

        private void ChatButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var friend = DataContext as Friend;
            if (friend == null) throw new NullReferenceException("Невозможно открыть чат с пользователем.");
            CommandManager.OpenFriendChatCommand.DoExecute(friend);
            IsOpen = false;
        }

        private void DeleteButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _deleteStateGrid.Visibility = Visibility.Visible;
        }

        private void YesButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var friend = DataContext as Friend;
            CommandManager.DeleteFriendCommand.DoExecute(friend);
            IsOpen = false;
            _deleteStateGrid.Visibility = Visibility.Collapsed;
        }

        private void NoButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _deleteStateGrid.Visibility = Visibility.Collapsed;
        }
    }
}
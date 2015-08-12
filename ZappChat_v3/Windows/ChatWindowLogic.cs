using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Windows
{
    public partial class ChatWindow : Window, INotifyPropertyChanged
    {
        private void AppShutdown(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void TestDB()
        {
            var frien1 = new Friend
            {
                ChatMemberId = "1",
                Name = "Вася",
                Type = ChatElementType.Friend
            };
            var frien2 = new Friend
            {
                ChatMemberId = "2",
                Name = "Стёпа",
                Type = ChatElementType.Friend
            };

            var frien3 = new Friend
            {
                ChatMemberId = "3",
                Name = "Рыся",
                Type = ChatElementType.Friend
            };

            var frien4 = new Friend
            {
                ChatMemberId = "4",
                Name = "Гоша",
                Type = ChatElementType.Friend
            };

            var frien5 = new Friend
            {
                ChatMemberId = "5",
                Name = "Пумба",
                Type = ChatElementType.Friend
            };
            var group1 = new Group
            {
                ChatMemberId = "1",
                Name = "Ребятки",
                Type = ChatElementType.Group,
                FriendList = new List<Friend> {frien1, frien2, frien4}
            };
            var group2 = new Group
            {
                ChatMemberId = "2",
                Name = "Dota",
                Type = ChatElementType.Group,
                FriendList = new List<Friend>{frien1, frien5}
            };
            DbContentManager.Instance.Groups.AddRange(new[] {group1, group2});
            DbContentManager.Instance.Friends.Add(frien3);
            DbContentManager.Instance.SaveChanges();

        }
    }
}

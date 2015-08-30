using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Windows
{
    /// <summary>
    /// Interaction logic for TEST_addFriend.xaml
    /// </summary>
    public partial class TEST_addFriend : Window
    {
        private ObservableCollection<Friend> _friends;
        public TEST_addFriend()
        {
            InitializeComponent();
        }

        public string NameFriend { get; set; }

        public TEST_addFriend(ObservableCollection<Friend> friends) : this()
        {
            _friends = friends;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(NameFriend)) return;
            var id = _friends.Max(f=>long.Parse(f.ChatMemberId)) + 1;
            var friend = new Friend
            {
                ChatMemberId = id.ToString(),
                Name = NameFriend,
                Type = ChatElementType.Friend
            };
            _friends.Add(friend);
            DbContentManager.Instance.Friends.Add(friend);
            DbContentManager.Instance.SaveChanges();
            Sostoyanie.Text = $"{friend.Name} добавлен";
            NameFriend = "";
        }
    }
}

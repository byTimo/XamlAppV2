using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using ZappChat_v3.Annotations;
using ZappChat_v3.Controls;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Windows
{
    public class ChatWindowModel : INotifyPropertyChanged
    {
        private ObservableCollection<Friend> _friends;
        private ObservableCollection<Group> _groups;
        private UserControl _currentContent;

        public ChatWindowModel()
        {
            FriendCollection =
                new ObservableCollection<Friend>(DbContentManager.Instance.Friends.Include(f => f.MembershipGroups));
            GroupCollection =
                new ObservableCollection<Group>(DbContentManager.Instance.Groups.Include(g => g.FriendList));
            CommandManager.CloseCurrentContent += () => MainContent = null;
            CommandManager.GroupCreateOpenCommand.Action += GroupCreateCommandOnAction;
            CommandManager.GroupCreateCommand.ParameterizedAction +=GroupCreateCommandOnParameterizedAction;
            CommandManager.GroupSettingOpenCommand.ParameterizedAction += GroupSettingOpenCallBack;
            CommandManager.GroupDeleteCommand.ParameterizedAction += GroupDeleteCommandOnParameterizedAction;
            CommandManager.AddFriendInGroupCommand.ParameterizedAction +=AddFriendInGroupCommandOnParameterizedAction;
            CommandManager.AddFriendCommand.Action += AddFriendCommandOnAction;
        }

        private void AddFriendCommandOnAction()
        {
            var add = new TEST_addFriend(FriendCollection);
            add.ShowDialog();
            add.Close();
        }

        public ObservableCollection<Friend> FriendCollection
        {
            get { return _friends; }
            set
            {
                _friends = value;
                OnPropertyChanged(nameof(FriendCollection));
            }
        }

        public ObservableCollection<Group> GroupCollection
        {
            get { return _groups; }
            set
            {
                _groups = value;
                OnPropertyChanged(nameof(GroupCollection));
            }
        }

        public UserControl MainContent
        {
            get { return _currentContent; }
            set
            {
                _currentContent = value;
                OnPropertyChanged(nameof(MainContent));
            }
        }

        private void GroupCreateCommandOnAction()
        {
            MainContent = new GroupCreate();
        }

        private void GroupCreateCommandOnParameterizedAction(object o)
        {
            var group = o as Group;
            DbContentManager.Instance.Groups.Add(group);
            DbContentManager.Instance.SaveChanges();
            GroupCollection.Add(group);
            MainContent = null;
        }

        private void GroupSettingOpenCallBack(object param)
        {
            if(param == null) throw new NullReferenceException();
            var group = GroupCollection.First(g => g.ChatMemberId.Equals(param as string));
            MainContent = new GroupSettingContent(group);
        }

        private void GroupDeleteCommandOnParameterizedAction(object o)
        {
            var group = GroupCollection.First(g => g.ChatMemberId.Equals(o as string));
            DbContentManager.Instance.Groups.Remove(group);
            DbContentManager.Instance.SaveChanges();
            GroupCollection.Remove(group);
            MainContent = null;
        }

        private void AddFriendInGroupCommandOnParameterizedAction(object o)
        {
            var group = GroupCollection.First(g => g.ChatMemberId.Equals(o as string));
            var add = new TEST_addFriendInGroup(group);
            add.ShowDialog();
            add.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

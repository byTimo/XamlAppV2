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
            CommandManager.GroupSettingOpenCommand.ParameterizedAction += GroupSettingOpenCallBack;
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
        private void GroupSettingOpenCallBack(object param)
        {
            if(param == null) throw new NullReferenceException();
            var group = GroupCollection.First(g => g.ChatMemberId == (string)param);
            MainContent = new GroupSettingContent(group);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

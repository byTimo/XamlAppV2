using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using ZappChat_v3.Annotations;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Windows
{
    public partial class ChatWindowModel : INotifyPropertyChanged
    {
        private readonly ChatWindow _chatWindow;
        private ObservableCollection<Friend> _friends;
        private ObservableCollection<Group> _groups;
        private UserControl _currentContent;

        public ChatWindowModel(ChatWindow window)
        {
            _chatWindow = window;
            BindCommandCallbacks();

            FriendCollection =
                new ObservableCollection<Friend>(DbContentManager.Instance.Friends.Include(f => f.MembershipGroups));
                        DbContentManager.Instance.SaveChanges();
            GroupCollection =
                new ObservableCollection<Group>(DbContentManager.Instance.Groups.Include(g => g.FriendList));
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

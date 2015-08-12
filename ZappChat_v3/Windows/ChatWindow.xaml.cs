using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Windows;
using ZappChat_v3.Annotations;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Windows
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Friend> _friends;
        private ObservableCollection<Group> _groups; 
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

        public ChatWindow()
        {
            InitializeComponent();
            //TestDB();
            FriendCollection = new ObservableCollection<Friend>(DbContentManager.Instance.Friends.Include(f=>f.MembershipGroups));
            GroupCollection = new ObservableCollection<Group>(DbContentManager.Instance.Groups.Include(g=>g.FriendList));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

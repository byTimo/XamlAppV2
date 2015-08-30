using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ZappChat_v3.Annotations;
using ZappChat_v3.Core;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Windows
{
    /// <summary>
    /// Interaction logic for TEST_addFriendInGroup.xaml
    /// </summary>
    public partial class TEST_addFriendInGroup : Window, INotifyPropertyChanged
    {
        public TEST_addFriendInGroup()
        {
            InitializeComponent();
        }

        public TEST_addFriendInGroup(Group group) : this()
        {
            Group = group;
            DataContext = Group;
            Friends = new ObservableCollection<Friend>();
            foreach (var friend in DbContentManager.Instance.Friends)
            {
                if(!Group.FriendList.Contains(friend))
                    Friends.Add(friend);
            }
        }

        private Group _group;
        private ObservableCollection<Friend> _friends;

        public Group Group
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged(nameof(Group));
            }
        }

        public ObservableCollection<Friend> Friends
        {
            get { return _friends; }
            set
            {
                _friends = value;
                OnPropertyChanged(nameof(Friends));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var listBoxItem = Support.FindAnchestor<ListBoxItem>((DependencyObject) e.OriginalSource);
            var friend = listBoxItem.DataContext as Friend;

            Group.Friends.Add(friend);
            Group.FriendList.Add(friend);
            friend.MembershipGroups.Add(Group);
            DbContentManager.Instance.SaveChanges();
            Friends.Remove(friend);
        }
    }
}

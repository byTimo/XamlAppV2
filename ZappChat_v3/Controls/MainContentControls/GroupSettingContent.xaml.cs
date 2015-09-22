using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ZappChat_v3.Annotations;
using ZappChat_v3.Core;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Controls.MainContentControls
{
    /// <summary>
    /// Interaction logic for GroupSettingContent.xaml
    /// </summary>
    public partial class GroupSettingContent : UserControl, INotifyPropertyChanged
    {
        private Group _group;

        public GroupSettingContent()
        {
            InitializeComponent();
        }

        public GroupSettingContent(Group group) : this()
        {
            Group = group;
            FriendListView.ItemsSource = Group.FriendList;
            var views = (CollectionView)CollectionViewSource.GetDefaultView(FriendListView.ItemsSource);
            views.Filter = UserFilter;
        }

        public Group Group
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged(nameof(Group));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GroupName.IsReadOnly = false;
            Keyboard.Focus(GroupName);
        }

        private void GroupName_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            GroupNameChange();
        }

        private void GroupName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key != Key.Enter) return;
            Keyboard.Focus(GruopNameBlock);
            Keyboard.ClearFocus();
        }

        private void GroupNameChange()
        {
            GroupName.IsReadOnly = true;
            DbContentManager.Instance.SaveChanges();

        }

        private void Bn_OnClick(object sender, RoutedEventArgs e)
        {
            var listBoxItem = Support.FindAnchestor<ListBoxItem>((DependencyObject) e.OriginalSource);
            var friend = listBoxItem.DataContext as Friend;

            Group.FriendList.Remove(friend);
            friend.MembershipGroups.Remove(Group);
            DbContentManager.Instance.SaveChanges();
        }

        private bool UserFilter(object item)
        {
            if (string.IsNullOrEmpty(FindTextBox.Text))
                return true;
            var friend = item as Friend;
            return friend.Name.IndexOf(FindTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   friend.LastName.IndexOf(FindTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }


        private void FindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(FriendListView.ItemsSource).Refresh();
        }

        private void FriendTextBoxLable_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Keyboard.Focus(FindTextBox);
        }

        private void FindTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            FindTextBoxLable.Visibility = Visibility.Collapsed;
        }

        private void FindTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if(FindTextBox.Text == "") FindTextBoxLable.Visibility = Visibility.Visible;
        }
    }
}

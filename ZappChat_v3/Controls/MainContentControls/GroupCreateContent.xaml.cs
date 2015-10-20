using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ZappChat_v3.Core;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;
using CommandManager = ZappChat_v3.Core.Managers.CommandManager;

namespace ZappChat_v3.Controls.MainContentControls
{
    /// <summary>
    /// Interaction logic for GroupCreate.xaml
    /// </summary>
    public partial class GroupCreate : UserControl
    {
        private readonly List<Friend> _addedFriends = new List<Friend>(); 
        public GroupCreate()
        {
            InitializeComponent();
            Friends = new List<Friend>(DbContentManager.Instance.Friends);
            FriendListBox.ItemsSource = Friends;
            //var views = (CollectionView)CollectionViewSource.GetDefaultView(FriendListBox.ItemsSource);
            //views.Filter = UserFilter;
        }

        public string GroupName { get; set; }
        public IEnumerable<Friend> Friends { get; }

        //private bool UserFilter(object item)
        //{
        //    if (string.IsNullOrEmpty(FindTextBox.Text))
        //        return true;
        //    var friend = item as Friend;
        //    return friend.Name.IndexOf(FindTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
        //           friend.LastName.IndexOf(FindTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        //}
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(GroupName.Trim()))
            {
                MessageBox.Show("Введите название группы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var id = DbContentManager.Instance.Groups.Count();
            var newGroup = new Group()
            {
                ChatMemberId = id.ToString(),
                Name = GroupName.Trim(),
                Type = ChatElementType.Group,
            };
            foreach (var source in _addedFriends)
            {
                newGroup.FriendList.Add(source);
            }
            CommandManager.GetCommand("GroupCreate").DoExecute(newGroup);
        }

        //private void FriendTextBoxLable_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        //{
        //    Keyboard.Focus(FindTextBox);
        //}

        //private void FindTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    FindTextBoxLable.Visibility = Visibility.Collapsed;
        //}

        //private void FindTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    if(FindTextBox.Text.Equals(""))
        //        FindTextBoxLable.Visibility = Visibility.Visible;
        //}

        //private void FindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    CollectionViewSource.GetDefaultView(FriendListBox.ItemsSource).Refresh();
        //    CollectionViewSource.GetDefaultView(FriendListBox.ItemsSource).
        //    //var currentActiveItems = new List<object>
        //    foreach (var addedFriend in _addedFriends)
        //    {
        //        var index = FriendListBox.Items.IndexOf(addedFriend);
        //        var listviewitem = FriendListBox.ItemContainerGenerator.ContainerFromItem(addedFriend) as ListBoxItem;
        //        var test = listviewitem.DataContext as Friend;
        //    }


        //}

        private void Bn_Checked(object sender, RoutedEventArgs e)
        {
            var listBoxItem = Support.FindAnchestor<ListBoxItem>((DependencyObject)e.OriginalSource);
            var friend = listBoxItem.DataContext as Friend;
            if (friend == null) throw new NullReferenceException();
            if(!_addedFriends.Contains(friend))
                _addedFriends.Add(friend);
        }

        private void Bn_Unchecked(object sender, RoutedEventArgs e)
        {
            var listBoxItem = Support.FindAnchestor<ListBoxItem>((DependencyObject)e.OriginalSource);
            var friend = listBoxItem.DataContext as Friend;
            if (friend == null) throw new NullReferenceException();
            if(_addedFriends.Contains(friend))
                _addedFriends.Remove(friend);
        }
    }
}

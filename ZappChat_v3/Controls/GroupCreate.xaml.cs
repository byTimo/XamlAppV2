using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ZappChat_v3.Core;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Controls
{
    /// <summary>
    /// Interaction logic for GroupCreate.xaml
    /// </summary>
    public partial class GroupCreate : UserControl
    {
        private Dictionary<Friend, bool> _friendDictionary; 
        public GroupCreate()
        {
            InitializeComponent();
            _friendDictionary = new Dictionary<Friend, bool>();
            Friends = new List<Friend>(DbContentManager.Instance.Friends);
            foreach (var friend in Friends)
            {
                _friendDictionary.Add(friend, false);
            }
        }

        public string GroupName { get; set; }
        public IEnumerable<Friend> Friends { get; }

        private void Bn_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if(button == null) throw  new NullReferenceException();
            var listBoxItem = Support.FindAnchestor<ListBoxItem>((DependencyObject) e.OriginalSource);
            var friend = listBoxItem.DataContext as Friend;
            if(friend == null) throw new NullReferenceException();

            var isChecked = button.Content as string != "Добавить";
            if (isChecked)
            {
                _friendDictionary[friend] = false;
                button.Content = "Добавить";
                listBoxItem.Background = Brushes.Transparent;
            }
            else
            {
                _friendDictionary[friend] = true;
                button.Content = "Удалить";
                listBoxItem.Background = new SolidColorBrush(Color.FromRgb(255, 201, 22));
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(GroupName.Trim()))
            {
                MessageBox.Show("Введите название группы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var id = long.Parse(DbContentManager.Instance.Groups.Max(g => g.ChatMemberId)) + 1;
            var newGroup = new Group()
            {
                ChatMemberId = id.ToString(),
                Name = GroupName.Trim(),
                Type = ChatElementType.Group,
            };
            foreach (var source in _friendDictionary.Where(x => x.Value))
            {
                newGroup.FriendList.Add(source.Key);
            }
            CommandManager.GroupCreateCommand.DoExecute(newGroup);
        }
    }
}

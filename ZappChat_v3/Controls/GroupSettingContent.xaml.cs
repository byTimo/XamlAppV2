using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZappChat_v3.Annotations;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Controls
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
    }
}

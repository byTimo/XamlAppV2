using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using ZappChat_v3.Annotations;
using ZappChat_v3.Core.ChatElements;

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
    }
}

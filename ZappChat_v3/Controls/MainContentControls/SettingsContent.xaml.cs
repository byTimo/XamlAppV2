using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using ZappChat_v3.Annotations;

namespace ZappChat_v3.Controls.MainContentControls
{
    /// <summary>
    /// Interaction logic for SettingsContent.xaml
    /// </summary>
    public partial class SettingsContent : UserControl, INotifyPropertyChanged
    {
        public UserControl LastControl { get; }
        private SettingsContent()
        {
            InitializeComponent();
        }

        public SettingsContent(UserControl lastControl) : this()
        {
            LastControl = lastControl;
        }

        public UserControl Close()
        {
            return LastControl;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

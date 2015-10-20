using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using ZappChat_v3.Annotations;
using ZappChat_v3.Core.Managers.Notifications;

namespace ZappChat_v3.Controls
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class NotificationControl : UserControl, INotifyPropertyChanged
    {
        public NotificationControl()
        {
            InitializeComponent();
        }

        public Notification Notification { get; }
        public NotificationControl(Notification notifi) : this()
        {
            Notification = notifi;
            switch (notifi.Type)
            {
                case NotificationType.Always:
                    NotificationColor = Colors.Blue;
                    break;
                case NotificationType.Error:
                    NotificationColor = Colors.Red;
                    break;
                default:
                    NotificationColor = Colors.Black;
                    break;
            }
            DataContext = notifi;
        }

        private Color _notificationColor;
        public Color NotificationColor
        {
            get { return _notificationColor; }
            set
            {
                _notificationColor = value;
                OnPropertyChanged(nameof(NotificationColor));
            }
        }

        public SolidColorBrush Brush => new SolidColorBrush(NotificationColor);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

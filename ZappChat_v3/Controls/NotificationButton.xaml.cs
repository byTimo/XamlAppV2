using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZappChat_v3.Annotations;
using ZappChat_v3.Core.Managers.Notifications;

namespace ZappChat_v3.Controls
{
    /// <summary>
    /// Interaction logic for NotificationButton.xaml
    /// </summary>
    public partial class NotificationButton : UserControl, INotifyPropertyChanged
    {
        private readonly DispatcherTimer _closeNotificationTimer = new DispatcherTimer();

        private readonly ImageBrush always = new ImageBrush(new BitmapImage(new Uri(@"\Images\NotificationAlways.png", UriKind.Relative)));
        //private readonly ImageBrush error = new ImageBrush(new BitmapImage(new Uri("\\Images\\NotificationError.png")));
        //private readonly ImageBrush file = new ImageBrush(new BitmapImage(new Uri("\\Images\\NotificationFile.png")));
        private readonly ImageBrush normal = new ImageBrush(new BitmapImage(new Uri(@"\Images\NotificationStatic.png", UriKind.Relative)));

        public NotificationButton()
        {
            InitializeComponent();
            NotificationManager.NewNotification += NewNotificationCallback;
            _closeNotificationTimer.Interval = TimeSpan.FromSeconds(5);
            _closeNotificationTimer.Tick += (sender, args) =>
            {
                _closeNotificationTimer.Stop();
                NotButton.Background = normal;
                Notification = null;
            };
        }

        private NotificationControl _notification;


        public NotificationControl Notification
        {
            get { return _notification; }
            private set
            {
                _notification = value;
                if(_closeNotificationTimer.IsEnabled)
                    _closeNotificationTimer.Stop();
                _closeNotificationTimer.Start();
                switch (_notification.Notification.Type)
                {
                    case NotificationType.Always:
                        NotButton.Background = always;
                        break;
                    //case NotificationType.Error:
                    //    NotButton.Background = error;
                    //    break;
                    //default:
                    //    NotButton.Background = file;
                    //    break;
                }
                OnPropertyChanged(nameof(Notification));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NotificationManager.AddNewNotification("Клик", NotificationType.Always);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NewNotificationCallback(Notification notification)
        {
            Notification = new NotificationControl(notification);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

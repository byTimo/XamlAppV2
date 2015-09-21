using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using ZappChat_v3.Annotations;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;
using ZappChat_v3.Core.Messaging;

namespace ZappChat_v3.Controls.MainContentControls
{
    /// <summary>
    /// Interaction logic for FriendChatContent.xaml
    /// </summary>
    public partial class FriendChatContent : UserControl, INotifyPropertyChanged
    {
        private Friend _friend;
        public Friend Friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                OnPropertyChanged(nameof(Friend));
            }
        }


        public FriendChatContent()
        {
            InitializeComponent();
        }

        public FriendChatContent(string friendId) : this()
        {
            Friend =
                DbContentManager.Instance.Friends.First(f => f.ChatMemberId.Equals(friendId));
            DataContext = Friend;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var userText = UserInput.Text;
            if(string.IsNullOrWhiteSpace(userText)) return;
            var message = new Message()
            {
                Type = MessageType.Outgoing,
                Class = MessageClass.Text,
                Text = userText.Trim(),
                AuthorOrRecipient = _friend
            };
            Friend.Messages.Add(message);
            DbContentManager.Instance.SaveChanges();
            UserInput.Text = "";
        }
    }
}

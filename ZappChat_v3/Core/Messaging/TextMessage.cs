using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using ZappChat_v3.Annotations;
using ZappChat_v3.Core.ChatElements;

namespace ZappChat_v3.Core.Messaging
{
    public class TextMessage : IMessage
    {
        [Key]
        public long Id { get; set; }
        public MessageType Type { get; }
        public string Text { get; set; }

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }
        public virtual ChatMember Author { get; set; }

        public TextMessage(MessageType type)
        {
            Type = type;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
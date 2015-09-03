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

        public virtual Friend Author { get; set; }

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

        public override string ToString()
        {
            return $"{Text} - {Id}";
        }

        protected bool Equals(TextMessage other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TextMessage) obj);
        }
    }
}
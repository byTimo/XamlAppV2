using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using ZappChat_v3.Annotations;
using ZappChat_v3.Core.ChatElements;

namespace ZappChat_v3.Core.Messaging
{
    public class Message
    {
        [Key]
        public long Id { get; set; }

        public MessageType Type { get; set; }

        public MessageClass Class { get; set; }

        public string Text { get; set; }

        public virtual Friend AuthorOrRecipient { get; set; }

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

        protected bool Equals(Message other)
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
            return Equals((Message) obj);
        }
    }
}
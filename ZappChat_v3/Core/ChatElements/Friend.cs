using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZappChat_v3.Core.Messaging;

namespace ZappChat_v3.Core.ChatElements
{
    [Table("Friends", Schema = "dbo")]
    public class Friend : IChatMember
    {
        [Key]
        public string ChatMemberId { get; set; }

        public ChatElementType Type { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public ICollection<Group> MembershipGroups { get; set; }

        public virtual ICollection<IMessage> Messages { get; set; }

        public Friend()
        {
            MembershipGroups = new ObservableCollection<Group>();
            Messages = new ObservableCollection<IMessage>();
        }

        public override string ToString()
        {
            return $"{Name} {LastName} - {ChatMemberId}";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Friend) obj);
        }

        protected bool Equals(Friend other)
        {
            return string.Equals(ChatMemberId, other.ChatMemberId) && Type == other.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ChatMemberId?.GetHashCode() ?? 0)*397) ^ (int) Type;
            }
        }
    }
}

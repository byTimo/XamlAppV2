using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZappChat_v3.Core.ChatElements
{
    [Table("Groups",Schema = "dbo")]
    public class Group : IChatMember
    {
        [Key]
        public string ChatMemberId { get; set; }
        public ChatElementType Type { get; set; }
        public string Name { get; set; }
        public ICollection<Friend> FriendList { get; set; }

        public Group()
        {
            FriendList = new ObservableCollection<Friend>();
        }

        public override string ToString()
        {
            return $"{Name} - {ChatMemberId}";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Friend)obj);
        }

        protected bool Equals(Friend other)
        {
            return string.Equals(ChatMemberId, other.ChatMemberId) && Type == other.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ChatMemberId?.GetHashCode() ?? 0) * 397) ^ (int)Type;
            }
        }
    }
}
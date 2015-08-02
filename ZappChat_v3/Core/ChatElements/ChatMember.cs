using System;

namespace ZappChat_v3.Core.ChatElements
{
    public abstract class ChatMember
    {
        public string ChatMemberId { get; set; }
        public ChatElementType Type { get; set; }

        protected bool Equals(ChatMember other)
        {
            return string.Equals(ChatMemberId, other.ChatMemberId) && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ChatMember) obj);
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

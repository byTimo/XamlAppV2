namespace ZappChat_v3.Core.ChatElements
{
    public abstract class ChatMember
    {
        public static long ChatMemberId { get; set; }
        public ChatElementType Type { get; set; }
    }
}

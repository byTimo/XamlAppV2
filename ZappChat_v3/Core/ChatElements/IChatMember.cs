using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZappChat_v3.Core.ChatElements
{
    public interface IChatMember
    {
        [Key]
        string ChatMemberId { get; set; }
        ChatElementType Type { get; set; }
        string Name { get; set; }
    }
}

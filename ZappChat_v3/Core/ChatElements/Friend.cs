using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ZappChat_v3.Core.Messaging;

namespace ZappChat_v3.Core.ChatElements
{
    [Table("Friends", Schema = "dbo")]
    public class Friend : ChatMember
    {
        public ICollection<Group> MembershipGroups { get; set; }
        public virtual ICollection<IMessage> Messages { get; set; } 

        public Friend()
        {
            MembershipGroups = new List<Group>();
            Messages = new List<IMessage>();
        }
    }
}

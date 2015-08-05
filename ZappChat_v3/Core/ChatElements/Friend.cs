using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZappChat_v3.Core.ChatElements
{
    [Table("Friends", Schema = "dbo")]
    public class Friend : ChatMember
    {
        public List<Group> MembershipGroups { get; set; }

        public Friend()
        {
            MembershipGroups = new List<Group>();
        }
    }
}

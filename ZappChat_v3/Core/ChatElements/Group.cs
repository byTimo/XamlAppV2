using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZappChat_v3.Core.ChatElements
{
    [Table("Groups",Schema = "dbo")]
    public class Group : ChatMember
    {
        public List<Friend> FriendList { get; set; }

        public Group()
        {
            FriendList = new List<Friend>();
        }
    }
}
﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZappChat_v3.Core.ChatElements
{
    [Table("Groups",Schema = "dbo")]
    public class Group : ChatMember
    {
        public ICollection<Friend> FriendList { get; set; }
        [NotMapped]
        public ObservableCollection<Friend> Friends => new ObservableCollection<Friend>(FriendList); 

        public Group()
        {
            FriendList = new List<Friend>();
        }
    }
}
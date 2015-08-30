using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Windows.Media;
using ZappChat_v3.Core.ChatElements;

namespace ZappChat_v3.Core.Managers
{
    public static class DbContentManager
    {
        public sealed class ZappDbContext : DbContext
        {
            public ZappDbContext() : base(ZappDbConnectionString)
            {
                FileManager.CheckExistsFiles();

            }

            public DbSet<Friend> Friends { get; set; }
            public DbSet<Group> Groups { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Group>().
                  HasMany(c => c.FriendList).
                  WithMany(p => p.MembershipGroups).
                  Map(
                   m =>
                   {
                       m.MapLeftKey("GroupId");
                       m.MapRightKey("FriendId");
                       m.ToTable("GroupFriend");
                   });
            }

        }

        private static string _zappDbConnectionString;
        private static ZappDbContext _instance;

        private static string ZappDbConnectionString => _zappDbConnectionString ??
                                                        (_zappDbConnectionString =
                                                            ConfigurationManager.ConnectionStrings["ZappDbConnectionString"].ConnectionString
                                                                .Replace("%APPDATA%",
                                                                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));

        public static ZappDbContext Instance => _instance ?? (_instance = new ZappDbContext());


        //Возможно это не понадобиться
        public static void UpdateInstance(ChatMember updatingChatMember)
        {
            switch (updatingChatMember.Type)
            {
                case ChatElementType.Friend:
                    UpdateFriend(updatingChatMember as Friend);
                    break;
                case ChatElementType.Group:
                    UpdateGroup(updatingChatMember as Group);
                    break;
            }
            Instance.SaveChanges();
        }

        private static void UpdateFriend(Friend updatingFriend)
        {
            var friend = Instance.Friends.First(f => f.ChatMemberId == updatingFriend.ChatMemberId);
            friend.Name = updatingFriend.Name;
            friend.MembershipGroups = updatingFriend.MembershipGroups;
            //@TODO--- остальные поля, когда появятся
        }

        private static void UpdateGroup(Group updatingGroup)
        {
            var group = Instance.Groups.First(g => g.ChatMemberId == updatingGroup.ChatMemberId);
            group.Name = updatingGroup.Name;
            group.FriendList = updatingGroup.FriendList;
            //@TODO ---
        }
    }
}

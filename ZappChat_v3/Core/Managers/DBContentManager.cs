using System.Data.Entity;
using System.IO;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Messaging;

namespace ZappChat_v3.Core.Managers
{
    public static class DbContentManager
    {
        public sealed class ZappDbContext : DbContext
        {
            public ZappDbContext() : base(ZappDbConnectionString)
            {
            }

            public DbSet<Friend> Friends { get; set; }
            public DbSet<Group> Groups { get; set; }
            public DbSet<Message> Messages { get; set; }

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
        public static string ZappDbConnectionString => Path.Combine(FileManager.ProfileFolder, "dbfile");

        private static ZappDbContext _instance;
        public static ZappDbContext Instance => _instance ?? (_instance = new ZappDbContext());
    }
}

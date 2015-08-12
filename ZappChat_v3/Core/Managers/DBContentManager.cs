using System;
using System.Configuration;
using System.Data.Entity;
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
    }
}

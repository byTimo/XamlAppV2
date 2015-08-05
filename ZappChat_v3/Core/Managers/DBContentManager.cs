using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ZappChat_v3.Core.ChatElements;

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

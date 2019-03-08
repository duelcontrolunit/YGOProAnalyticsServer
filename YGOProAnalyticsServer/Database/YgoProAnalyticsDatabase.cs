using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Database
{
    public class YgoProAnalyticsDatabase : DbContext
    {
        public DbSet<Banlist> Banlists { get; set; }
        public DbSet<Card> Cards { get; set; }

        public const string connectionString =
               @"Server=(localdb)\mssqllocaldb;
                 Database= YgoProAnalytics;
                 Trusted_Connection=True;
                 ConnectRetryCount=0";

        public YgoProAnalyticsDatabase(DbContextOptions<YgoProAnalyticsDatabase> options) : base(options)
        {

        }
    }
}

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
        public DbSet<Archetype> Archetypes { get; set; }
        public DbSet<ArchetypeStatistics> ArchetypeStatistics { get; set; }

        //cards
        public DbSet<Card> Cards { get; set; }
        public DbSet<MonsterCard> MonsterCards { get; set; }
        

        public const string connectionString =
               @"Server=(localdb)\mssqllocaldb;
                 Database= YgoProAnalytics;
                 Trusted_Connection=True;
                 ConnectRetryCount=0";

        public YgoProAnalyticsDatabase(DbContextOptions<YgoProAnalyticsDatabase> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ForbiddenCardBanlistJoin>()
              .HasKey(t => new { t.CardId, t.BanlistId });

            modelBuilder.Entity<LimitedCardBanlistJoin>()
              .HasKey(t => new { t.CardId, t.BanlistId });

            modelBuilder.Entity<SemiLimitedCardBanlistJoin>()
              .HasKey(t => new { t.CardId, t.BanlistId });

            base.OnModelCreating(modelBuilder);
        }
    }
}

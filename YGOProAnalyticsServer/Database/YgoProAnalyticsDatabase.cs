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
        public DbSet<PendulumMonsterCard> PendulumMonsterCards { get; set; }
        public DbSet<LinkMonsterCard> LinkMonsterCards { get; set; }

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

            modelBuilder.Entity<Card>()
                .HasOne(a => a.MonsterCard)
                .WithOne(b => b.Card)
                .HasForeignKey<MonsterCard>(c => c.CardId);

            modelBuilder.Entity<MonsterCard>()
                .HasOne(a => a.LinkMonsterCard)
                .WithOne(b => b.MonsterCard)
                .HasForeignKey<LinkMonsterCard>(c => c.MonsterCardId);

            modelBuilder.Entity<MonsterCard>()
                .HasOne(a => a.PendulumMonsterCard)
                .WithOne(b => b.MonsterCard)
                .HasForeignKey<PendulumMonsterCard>(c => c.MonsterCardId);

            modelBuilder.Entity<Card>()
                .HasOne(a => a.Archetype)
                .WithMany(b => b.Cards);

            base.OnModelCreating(modelBuilder);
        }
    }
}

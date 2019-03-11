using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServerTests.Database
{
    [TestFixture]
    class YgoProAnalyticsDatabaseTests
    {
        [Test]
        public void DatabaseExists_ReturnsTrue()
        {
            var optionsBuilder = new DbContextOptionsBuilder<YgoProAnalyticsDatabase>();
            optionsBuilder.UseSqlServer(YgoProAnalyticsDatabase.connectionString);
            using (YgoProAnalyticsDatabase db = new YgoProAnalyticsDatabase(optionsBuilder.Options))
            {
                Assert.IsTrue((db.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists());
            }
        }

        [Test]
        public void Dtest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<YgoProAnalyticsDatabase>();
            optionsBuilder.UseSqlServer(YgoProAnalyticsDatabase.connectionString);
            using (YgoProAnalyticsDatabase db = new YgoProAnalyticsDatabase(optionsBuilder.Options))
            {
                var banlist = db
                    .Banlists
                    .Include(Banlist.IncludeWithForbiddenCards)
                    .First();
                
                var y = banlist.ForbiddenCards.First();
                var x = 5;
            }
        }
    }
}

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
using YGOProAnalyticsServer.Services.Updaters;
using YGOProAnalyticsServer.Services.Downloaders;
using YGOProAnalyticsServer.Services.Builders;
using System.Threading.Tasks;

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
        public async Task Dtest()
        {
            //var x = new CardsDataToCardsAndArchetypesUpdater(new CardsDataDownloader(), new CardBuilder(), db);
            //await x.UpdateCardsAndArchetypes("https://db.ygoprodeck.com/api/v3/cardinfo.php");
        }
    }
}

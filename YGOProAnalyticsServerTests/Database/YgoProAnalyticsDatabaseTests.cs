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
        public async Task DatabaseExists_ReturnsTrue()
        {
            var optionsBuilder = new DbContextOptionsBuilder<YgoProAnalyticsDatabase>();
            optionsBuilder.UseSqlServer(YgoProAnalyticsDatabase.connectionString);
            using (YgoProAnalyticsDatabase db = new YgoProAnalyticsDatabase(optionsBuilder.Options))
            {
                var x = new CardsDataToCardsAndArchetypesUpdater(new CardsDataDownloader(), new CardBuilder(),db);
                await x.ConvertCards("https://db.ygoprodeck.com/api/v3/cardinfo.php");
                Assert.IsTrue((db.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists());
            }
        }

        [Test]
        public async Task Dtest()
        {
            //var x = new CardsDataToCardsAndArchetypesConverter(new CardsDataDownloader(), new MonsterCardBuilder());
            //await x.ConvertCards("https://db.ygoprodeck.com/api/v3/cardinfo.php");
            //ard x = new Card();
        }
    }
}

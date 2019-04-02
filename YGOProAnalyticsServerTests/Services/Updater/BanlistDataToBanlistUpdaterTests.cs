using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Builders;
using YGOProAnalyticsServer.Services.Downloaders;
using YGOProAnalyticsServer.Services.Updaters;

namespace YGOProAnalyticsServerTests.Services.Updater
{
    [TestFixture]
    class BanlistDataToBanlistUpdaterTests
    {
        [Test, Explicit]
        public async Task UpdateBanlists_SourcesShouldBeAvailable_DbInMemoryIsUpdated()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var dbContextSqliteOptions =
                new DbContextOptionsBuilder<YgoProAnalyticsDatabase>()
                .UseSqlite(connection)
                .ConfigureWarnings(x => x.Ignore(RelationalEventId.QueryClientEvaluationWarning))
                .Options;

            using (var db = new YgoProAnalyticsDatabase(dbContextSqliteOptions))
            {
                db.Database.EnsureCreated();

                var _updater = new CardsDataToCardsAndArchetypesUpdater(new CardsDataDownloader(), new CardBuilder(), db);
                await _updater.UpdateCardsAndArchetypes("https://db.ygoprodeck.com/api/v3/cardinfo.php");


                var updater = new BanlistDataToBanlistUpdater(db, new BanlistDataDownloader());
                await updater.UpdateBanlists("https://raw.githubusercontent.com/szefo09/updateYGOPro2/master/lflist.conf");
            }
               
        } 
    }
}

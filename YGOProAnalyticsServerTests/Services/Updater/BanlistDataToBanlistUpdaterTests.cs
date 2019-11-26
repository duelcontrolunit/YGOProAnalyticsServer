using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Builders;
using YGOProAnalyticsServer.Services.Downloaders;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;
using YGOProAnalyticsServer.Services.Updaters;
using YGOProAnalyticsServer.Services.Updaters.Interfaces;

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
                var newBanlists = await updater.UpdateBanlists("https://raw.githubusercontent.com/szefo09/updateYGOPro2/master/lflist.conf");
                Assert.Greater(newBanlists.Count(), 0);
            }
        }

        Mock<IBanlistDataDownloader> _banlistDataDownloaderMock;

        [SetUp]
        public void SetUp()
        {
            _banlistDataDownloaderMock = new Mock<IBanlistDataDownloader>();
            _banlistDataDownloaderMock
                .Setup(x => x.DownloadBanlistFromWebsite(It.IsAny<string>()))
                .ReturnsAsync(_getValidLflist());
        }

        [Test]
        public async Task UpdateBanlist_SourcesGiveUsValidData_DbContainValidNumberOfForbiddenAndLimitedAndSemiLimitedCards()
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
                var archetype = new Archetype(Archetype.Default, true);
                _addChangeOfHeart(db, archetype);
                _addChickenGame(db, archetype);
                _addTerraforming(db, archetype);
                await db.SaveChangesAsync();

                var updater = new BanlistDataToBanlistUpdater(db, _banlistDataDownloaderMock.Object);
                await updater.UpdateBanlists("https://raw.githubusercontent.com/szefo09/updateYGOPro2/master/lflist.conf");

                await _verifyForTCGBanlist(db);
                await _verifyForOCGBanlist(db);
            }
        }

        private static async Task _verifyForOCGBanlist(YgoProAnalyticsDatabase db)
        {
            var banlistOCG = await db
                .Banlists
                .Where(x => x.Name == "2019.01 OCG")
                .Include(Banlist.IncludeWithForbiddenCards)
                .Include(Banlist.IncludeWithLimitedCards)
                .Include(Banlist.IncludeWithSemiLimitedCards)
                .FirstAsync();

            Assert.AreEqual(0, banlistOCG.ForbiddenCardsJoin.Count());
            Assert.AreEqual(0, banlistOCG.LimitedCardsJoin.Count());
            Assert.AreEqual(0, banlistOCG.SemiLimitedCardsJoin.Count());
        }

        private static async Task _verifyForTCGBanlist(YgoProAnalyticsDatabase db)
        {
            var banlistTcg = await db
                                .Banlists
                                .Where(x => x.Name == "2019.01 TCG")
                                .Include(Banlist.IncludeWithForbiddenCards)
                                .Include(Banlist.IncludeWithLimitedCards)
                                .Include(Banlist.IncludeWithSemiLimitedCards)
                                .FirstAsync();

            Assert.AreEqual(2, banlistTcg.ForbiddenCardsJoin.Count());
            Assert.AreEqual(0, banlistTcg.LimitedCardsJoin.Count());
            Assert.AreEqual(1, banlistTcg.SemiLimitedCardsJoin.Count());
        }

        private static void _addTerraforming(YgoProAnalyticsDatabase db, Archetype archetype)
        {
            db.Cards.Add(
                Card.Create(
                    73628505,
                    "Terraforming",
                    "Terraforming",
                    "Spell Card",
                    "normal",
                    null,
                    null,
                    archetype
                )
            );
        }

        private static void _addChickenGame(YgoProAnalyticsDatabase db, Archetype archetype)
        {
            db.Cards.Add(
                Card.Create(
                    67616300,
                    "Chicken Game",
                    "Chicken Game",
                    "Spell Card",
                    "normal",
                    null,
                    null,
                    archetype
                )
            );
        }

        private static void _addChangeOfHeart(YgoProAnalyticsDatabase db, Archetype archetype)
        {
            db.Cards.Add(
                Card.Create(
                    4031928,
                    "Change of Heart",
                    "Description: Change of Heart",
                    "Spell Card",
                    "normal",
                    null,
                    null,
                    archetype
                )
            );
        }

        public string _getValidLflist()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("#[2019.01 TCG][2019.04 OCG][2019.04 Worlds][2019.01 Traditional][2019.04 Anime]");
            stringBuilder.AppendLine("!2019.01 TCG");
            stringBuilder.AppendLine("#Forbidden TCG");
            stringBuilder.AppendLine("4031928 0 --Change of Heart");
            stringBuilder.AppendLine("67616300 0 --Chicken Game");
            stringBuilder.AppendLine("#Limited TCG");
            stringBuilder.AppendLine("#Semi limited TCG");
            stringBuilder.AppendLine("73628505 2 --Terraforming");

            stringBuilder.AppendLine("!2019.01 OCG");
            stringBuilder.AppendLine("#Forbidden TCG");
            stringBuilder.AppendLine("#Limited OCG");
            stringBuilder.AppendLine("#Semi limited OCG");
            return stringBuilder.ToString();
        }
    }
}

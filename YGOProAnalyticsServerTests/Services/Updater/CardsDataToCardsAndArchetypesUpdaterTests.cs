using NUnit.Framework;
using System.Linq;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.Services.Builders;
using YGOProAnalyticsServer.Services.Downloaders;
using YGOProAnalyticsServer.Services.Updaters;
using YGOProAnalyticsServer.Services.Updaters.Interfaces;
using System.Threading.Tasks;
using Moq;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;
using YGOProAnalyticsServer.Services.Builders.Inferfaces;
using YGOProAnalyticsServer.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace YGOProAnalyticsServerTests.Services.Updater
{
    [TestFixture]
    class CardsDataToCardsAndArchetypesUpdaterTests
    {
        ICardsDataToCardsAndArchetypesUpdater _updater;
        Mock<ICardsDataDownloader> _downloaderMock;
        Mock<ICardBuilder> _cardBuilderMock;
        DbContextOptions<YgoProAnalyticsDatabase> _dbContextSqliteOptions;
        readonly Archetype _archetype = new Archetype("TestArchetype", true);
        
        [SetUp]
        public void SetUp()
        {
            _downloaderMock = new Mock<ICardsDataDownloader>();
            _downloaderMock
                .Setup(x => x.DownloadCardsFromWebsite("https://db.ygoprodeck.com/api/v3/cardinfo.php"))
                .ReturnsAsync(_getValidCardsDataFromApi());
            _cardBuilderMock = new Mock<ICardBuilder>();
            _cardBuilderMock
                .Setup(x => x.Build())
                .Returns(
                    Card.Create(
                        27551,
                        "Limit Reverse",
                        "Desc",
                        "Trap Card",
                        "race",
                        null,
                        null,
                        _archetype));
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            _dbContextSqliteOptions = new DbContextOptionsBuilder<YgoProAnalyticsDatabase>()
                .UseSqlite(connection)
                .ConfigureWarnings(x => x.Ignore(RelationalEventId.QueryClientEvaluationWarning))
                .Options;
        }

        [Test]
        public async Task UpdateCardsAndArchetypes_WeGetCardsDataAsJSon_CardsAndArchetypesTablesUpdated()
        {
            using (var db = new YgoProAnalyticsDatabase(_dbContextSqliteOptions))
            {
                db.Database.EnsureCreated();

                _updater = new CardsDataToCardsAndArchetypesUpdater(_downloaderMock.Object, _cardBuilderMock.Object, db);
                await _updater.UpdateCardsAndArchetypes("https://db.ygoprodeck.com/api/v3/cardinfo.php");

                Assert.NotZero(db.Cards.Count(), "Cards table should not be empty.");
                Assert.NotZero(db.Archetypes.Count(), "Archetypes table should not be empty.");
            }
        }

        /// <summary>
        /// Time expensive integrity test
        /// </summary>
        [Test, Explicit]
        public async Task UpdateCardsAndArchetypes_SourceAreAvailable_CardsAndArchetypesTablesUpdated()
        {
            using (var db = new YgoProAnalyticsDatabase(_dbContextSqliteOptions))
            {
                db.Database.EnsureCreated();
                _updater = new CardsDataToCardsAndArchetypesUpdater(new CardsDataDownloader(), new CardBuilder(), db);
                await _updater.UpdateCardsAndArchetypes("https://db.ygoprodeck.com/api/v3/cardinfo.php");

                Assert.NotZero(db.Cards.Count());
            }  
        }

        private string _getValidCardsDataFromApi()
        {
            return "[[{\"id\":\"27551\",\"name\":\"Limit Reverse\",\"desc\":\"Target 1 monster with 1000 or less ATK in your Graveyard; Special Summon it in Attack Position. If the target is changed to Defense Position, destroy it and this card. When this card leaves the field, destroy the target. When the target is destroyed, destroy this card.\",\"atk\":null,\"def\":null,\"type\":\"Trap Card\",\"level\":\"0\",\"race\":\"Continuous\",\"attribute\":\"0\",\"scale\":null,\"linkval\":null,\"linkmarkers\":null,\"archetype\":null,\"set_tag\":\"YS12-EN037,YS11-EN039,LODT-EN063,5DS2-EN037,\",\"setcode\":\"Starter Deck: Xyz Symphony,Starter Deck: Dawn of the Xyz,Light of Destruction,Starter Deck: Yu-Gi-Oh! 5D's 2009,\",\"ban_tcg\":null,\"ban_ocg\":null,\"ban_goat\":null,\"image_url\":null,\"image_url_small\":null}]]";
        }
    }
}

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
            _downloaderMock
    .Setup(x => x.DownloadCardsFromWebsite("http://eeriecode.altervista.org/tools/get_beta_cards.php"))
    .ReturnsAsync(_getValidCardsDataFromBetaApi());
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
        private string _getValidCardsDataFromBetaApi()
        {
            return "[{\"name\":\"Flash Charge Dragon\",\"ucode\":\"0\",\"ocode\":\"95372220\"},{\"name\":\"Mischief of the Time Goddess\",\"ucode\":\"0\",\"ocode\":\"92182447\"},{\"name\":\"Clock Lizard\",\"ucode\":\"0\",\"ocode\":\"51476410\"},{\"name\":\"Borrel Supplier\",\"ucode\":\"100336023\",\"ocode\":\"30131474\"},{\"name\":\"Checksum Dragon\",\"ucode\":\"100336006\",\"ocode\":\"94136469\"},{\"name\":\"Absorouter Dragon\",\"ucode\":\"100336005\",\"ocode\":\"67748760\"},{\"name\":\"Exploderokket Dragon\",\"ucode\":\"100336004\",\"ocode\":\"31353051\"},{\"name\":\"Salamangreat Pyro Phoenix\",\"ucode\":\"101010039\",\"ocode\":\"31313405\"},{\"name\":\"Borreload Furious Dragon\",\"ucode\":\"100336051\",\"ocode\":\"92892239\"},{\"name\":\"Sawnborrel Dragon\",\"ucode\":\"100336052\",\"ocode\":\"29296344\"},{\"name\":\"Rapid Trigger\",\"ucode\":\"100336024\",\"ocode\":\"67526112\"},{\"name\":\"Topologic Zerovoros\",\"ucode\":\"100336041\",\"ocode\":\"66403530\"},{\"name\":\"Firewall Dragon Darkfluid\",\"ucode\":\"101010037\",\"ocode\":\"68934651\"},{\"name\":\"Starliege Dragon Seyfert\",\"ucode\":\"101010014\",\"ocode\":\"15381421\"},{\"name\":\"Galaxy Satellite Dragon\",\"ucode\":\"101010047\",\"ocode\":\"92362073\"},{\"name\":\"True Exodia\",\"ucode\":\"100250001\",\"ocode\":\"0\"},{\"name\":\"Seraphim Papillon\",\"ucode\":\"101010050\",\"ocode\":\"91140491\"},{\"name\":\"Silverrokket Dragon\",\"ucode\":\"100336001\",\"ocode\":\"32476603\"},{\"name\":\"Tenyi Dragon Ajna\",\"ucode\":\"101010019\",\"ocode\":\"87052196\"},{\"name\":\"Dragon Ogre Deity of Tenyi\",\"ucode\":\"101010035\",\"ocode\":\"5041348\"},{\"name\":\"Rokket Tracer\",\"ucode\":\"100336002\",\"ocode\":\"68464358\"},{\"name\":\"Rokket Recharger\",\"ucode\":\"100336003\",\"ocode\":\"5969957\"},{\"name\":\"Striker Dragon\",\"ucode\":\"100200165\",\"ocode\":\"73539069\"},{\"name\":\"Kongrade, Primate of Conglomerates\",\"ucode\":\"101010024\",\"ocode\":\"49729312\"},{\"name\":\"Granite Loyalist\",\"ucode\":\"101010036\",\"ocode\":\"32530043\"},{\"name\":\"Zero Day Blaster\",\"ucode\":\"100336033\",\"ocode\":\"93014827\"},{\"name\":\"Gunslinger Execution\",\"ucode\":\"100336034\",\"ocode\":\"20419926\"},{\"name\":\"Marincess Mandarin\",\"ucode\":\"101010002\",\"ocode\":\"28174796\"},{\"name\":\"Marincess Crown Tail\",\"ucode\":\"101010003\",\"ocode\":\"54569495\"},{\"name\":\"Marincess Blue Tang\",\"ucode\":\"101010004\",\"ocode\":\"91953000\"},{\"name\":\"Marincess Crystal Heart\",\"ucode\":\"101010040\",\"ocode\":\"67712104\"},{\"name\":\"Marincess Wonder Heart\",\"ucode\":\"101010041\",\"ocode\":\"94207108\"},{\"name\":\"Marincess Sea Angel\",\"ucode\":\"101010042\",\"ocode\":\"30691817\"},{\"name\":\"Marincess Battle Ocean\",\"ucode\":\"101010053\",\"ocode\":\"91027843\"},{\"name\":\"Marincess Snow\",\"ucode\":\"101010067\",\"ocode\":\"80627281\"},{\"name\":\"Marincess Cascade\",\"ucode\":\"101010068\",\"ocode\":\"27012990\"},{\"name\":\"Hakai Douji Arha\",\"ucode\":\"101010008\",\"ocode\":\"26236560\"},{\"name\":\"Hakai Douji Rakia\",\"ucode\":\"101010009\",\"ocode\":\"53624265\"},{\"name\":\"Hakaishin no Magatama\",\"ucode\":\"101010010\",\"ocode\":\"89019964\"},{\"name\":\"Hakaishin Ragia\",\"ucode\":\"101010043\",\"ocode\":\"67680512\"},{\"name\":\"Hakaishin Arba\",\"ucode\":\"101010044\",\"ocode\":\"93084621\"},{\"name\":\"Hakai Souohshin Raigou\",\"ucode\":\"101010045\",\"ocode\":\"29479265\"},{\"name\":\"The Shackles of Souoh\",\"ucode\":\"101010054\",\"ocode\":\"27412542\"},{\"name\":\"Wail of the Hakaishin\",\"ucode\":\"101010055\",\"ocode\":\"54807656\"},{\"name\":\"Hakai Homily\",\"ucode\":\"101010069\",\"ocode\":\"53417695\"},{\"name\":\"Hakai Dual Dirge\",\"ucode\":\"101010070\",\"ocode\":\"80801743\"},{\"name\":\"Processlayer Sigma\",\"ucode\":\"100423001\",\"ocode\":\"27182739\"},{\"name\":\"Processlayer Nabla\",\"ucode\":\"100423002\",\"ocode\":\"53577438\"}]";
        }

    }
}

using NUnit.Framework;
using YGOProAnalyticsServer.Services.Converters;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Database;
using System.Linq;
using YGOProAnalyticsServer.DbModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Threading.Tasks;
using YGOProAnalyticsServer;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;
using Moq;

namespace YGOProAnalyticsServerTests.Services.Converters
{
    [TestFixture]
    class YDKToDecklistConverterTests
    {
        Mock<IAdminConfig> _adminConfigMock;
        Mock<ICardsDataDownloader> _downloaderMock;

        [SetUp]
        public void SetUp()
        {
            _downloaderMock = new Mock<ICardsDataDownloader>();
            _downloaderMock
    .Setup(x => x.DownloadCardsFromWebsite("http://eeriecode.altervista.org/tools/get_beta_cards.php"))
    .ReturnsAsync(_getValidCardsDataFromBetaApi());
            _adminConfigMock = new Mock<IAdminConfig>();
            _adminConfigMock.Setup(x => x.BetaToOfficialCardApiURL).Returns("http://eeriecode.altervista.org/tools/get_beta_cards.php");
        }

        IYDKToDecklistConverter _converter;
        [Test]
        public async Task GetProperYDKString_ReturnDecklistWithFilledFieldsAsync()
        {
            using (var dbInMemory = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                dbInMemory.Database.EnsureCreated();
                var archetype = new Archetype("Neutral", true);
                _addPSYFrameDriver(dbInMemory, archetype);
                _addMokeyMokeyKing(dbInMemory, archetype);
                _addChecksumDragon(dbInMemory, archetype);
                _addPankratops(dbInMemory, archetype);
                await dbInMemory.SaveChangesAsync();

                _converter = new YDKToDecklistConverter(dbInMemory, _downloaderMock.Object, _adminConfigMock.Object);
                var decklist = _converter.Convert(GetProperYDKString());
                decklist.Name = "Test";
                decklist.Archetype = new Archetype(Archetype.Default, true);
                dbInMemory.Decklists.Add(decklist);
                await dbInMemory.SaveChangesAsync();
                var decklistFromDb = dbInMemory.Decklists.First();

                Assert.Multiple(() =>
                {
                    //tests if a beta card (Cheksum dragon with betaid:100336006) is converted to 94136469
                    Assert.IsNotNull(decklistFromDb.MainDeck.FirstOrDefault(x => x.PassCode == 94136469));
                    Assert.AreEqual(decklistFromDb.MainDeck, decklist.MainDeck);
                    Assert.AreEqual(decklistFromDb.ExtraDeck, decklist.ExtraDeck);
                    Assert.AreEqual(decklistFromDb.SideDeck, decklist.SideDeck);
                });
            }
        }
        private string GetProperYDKString()
        {
            return "#created by ygopro2\r\n#main\r\n100336006\r\n59509952\r\n80701178\r\n80701178\r\n18474999\r\n95492061\r\n53303460\r\n53303460\r\n90307777\r\n82085295\r\n82085295\r\n16229315\r\n16229315\r\n49036338\r\n52068432\r\n25857246\r\n4810828\r\n65877963\r\n65877963\r\n65877963\r\n26674724\r\n26674724\r\n26674724\r\n77235086\r\n77235086\r\n89463537\r\n99185129\r\n99185129\r\n38814750\r\n38814750\r\n38814750\r\n96729612\r\n96729612\r\n96729612\r\n32807846\r\n51124303\r\n51124303\r\n14735698\r\n86758915\r\n97211663\r\n97211663\r\n24224830\r\n24224830\r\n24224830\r\n#extra\r\n13803864\r\n80532587\r\n80773359\r\n41517789\r\n74586817\r\n79606837\r\n79606837\r\n79606837\r\n55863245\r\n57707471\r\n6983839\r\n46772449\r\n8809344\r\n87054946\r\n2857636\r\n!side\r\n10000080\r\n10000080\r\n10000080\r\n82385847\r\n82385847\r\n13974207\r\n13974207\r\n13974207\r\n52738610\r\n43898403\r\n43898403\r\n43898403\r\n15693423\r\n15693423\r\n15693423";
        }
        private DbContextOptions<T> _getOptionsForSqlInMemoryTesting<T>() where T : DbContext
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            return new DbContextOptionsBuilder<T>()
                .UseSqlite(connection)
                .ConfigureWarnings(x => x.Ignore(RelationalEventId.QueryClientEvaluationWarning))
                .Options;
        }
        private static void _addPSYFrameDriver(YgoProAnalyticsDatabase db, Archetype archetype)
        {
            db.Cards.Add(
                Card.Create(
                    49036338,
                    "PSY-Frame Driver",
                    "Description: PSY-Frame Driver",
                    "Monster Card",
                    "normal",
                    null,
                    null,
                    archetype
                )
            );
        }
        private static void _addChecksumDragon(YgoProAnalyticsDatabase db, Archetype archetype)
        {
            db.Cards.Add(
                Card.Create(
                    94136469,
                    "Checksum Dragon",
                    "Description: Checksum Dragon",
                    "Monster Card",
                    "normal",
                    null,
                    null,
                    archetype
                )
            );
        }
        private static void _addMokeyMokeyKing(YgoProAnalyticsDatabase db, Archetype archetype)
        {
            db.Cards.Add(
                Card.Create(
                    13803864,
                    "Mokey Mokey King",
                    "Description: Mokey Mokey King",
                    "Fusion Monster Card",
                    "normal",
                    null,
                    null,
                    archetype
                )
            );
        }
        private static void _addPankratops(YgoProAnalyticsDatabase db, Archetype archetype)
        {
            db.Cards.Add(
                Card.Create(
                    82385847,
                    "Dinowrestler Pankratops",
                    "Description: Dinowrestler Pankratops",
                    "Effect Monster Card",
                    "normal",
                    null,
                    null,
                    archetype
                )
            );
        }
        private string _getValidCardsDataFromBetaApi()
        {
            return "[{\"name\":\"Flash Charge Dragon\",\"ucode\":\"0\",\"ocode\":\"95372220\"},{\"name\":\"Mischief of the Time Goddess\",\"ucode\":\"0\",\"ocode\":\"92182447\"},{\"name\":\"Clock Lizard\",\"ucode\":\"0\",\"ocode\":\"51476410\"},{\"name\":\"Borrel Supplier\",\"ucode\":\"100336023\",\"ocode\":\"30131474\"},{\"name\":\"Checksum Dragon\",\"ucode\":\"100336006\",\"ocode\":\"94136469\"},{\"name\":\"Absorouter Dragon\",\"ucode\":\"100336005\",\"ocode\":\"67748760\"},{\"name\":\"Exploderokket Dragon\",\"ucode\":\"100336004\",\"ocode\":\"31353051\"},{\"name\":\"Salamangreat Pyro Phoenix\",\"ucode\":\"101010039\",\"ocode\":\"31313405\"},{\"name\":\"Borreload Furious Dragon\",\"ucode\":\"100336051\",\"ocode\":\"92892239\"},{\"name\":\"Sawnborrel Dragon\",\"ucode\":\"100336052\",\"ocode\":\"29296344\"},{\"name\":\"Rapid Trigger\",\"ucode\":\"100336024\",\"ocode\":\"67526112\"},{\"name\":\"Topologic Zerovoros\",\"ucode\":\"100336041\",\"ocode\":\"66403530\"},{\"name\":\"Firewall Dragon Darkfluid\",\"ucode\":\"101010037\",\"ocode\":\"68934651\"},{\"name\":\"Starliege Dragon Seyfert\",\"ucode\":\"101010014\",\"ocode\":\"15381421\"},{\"name\":\"Galaxy Satellite Dragon\",\"ucode\":\"101010047\",\"ocode\":\"92362073\"},{\"name\":\"True Exodia\",\"ucode\":\"100250001\",\"ocode\":\"0\"},{\"name\":\"Seraphim Papillon\",\"ucode\":\"101010050\",\"ocode\":\"91140491\"},{\"name\":\"Silverrokket Dragon\",\"ucode\":\"100336001\",\"ocode\":\"32476603\"},{\"name\":\"Tenyi Dragon Ajna\",\"ucode\":\"101010019\",\"ocode\":\"87052196\"},{\"name\":\"Dragon Ogre Deity of Tenyi\",\"ucode\":\"101010035\",\"ocode\":\"5041348\"},{\"name\":\"Rokket Tracer\",\"ucode\":\"100336002\",\"ocode\":\"68464358\"},{\"name\":\"Rokket Recharger\",\"ucode\":\"100336003\",\"ocode\":\"5969957\"},{\"name\":\"Striker Dragon\",\"ucode\":\"100200165\",\"ocode\":\"73539069\"},{\"name\":\"Kongrade, Primate of Conglomerates\",\"ucode\":\"101010024\",\"ocode\":\"49729312\"},{\"name\":\"Granite Loyalist\",\"ucode\":\"101010036\",\"ocode\":\"32530043\"},{\"name\":\"Zero Day Blaster\",\"ucode\":\"100336033\",\"ocode\":\"93014827\"},{\"name\":\"Gunslinger Execution\",\"ucode\":\"100336034\",\"ocode\":\"20419926\"},{\"name\":\"Marincess Mandarin\",\"ucode\":\"101010002\",\"ocode\":\"28174796\"},{\"name\":\"Marincess Crown Tail\",\"ucode\":\"101010003\",\"ocode\":\"54569495\"},{\"name\":\"Marincess Blue Tang\",\"ucode\":\"101010004\",\"ocode\":\"91953000\"},{\"name\":\"Marincess Crystal Heart\",\"ucode\":\"101010040\",\"ocode\":\"67712104\"},{\"name\":\"Marincess Wonder Heart\",\"ucode\":\"101010041\",\"ocode\":\"94207108\"},{\"name\":\"Marincess Sea Angel\",\"ucode\":\"101010042\",\"ocode\":\"30691817\"},{\"name\":\"Marincess Battle Ocean\",\"ucode\":\"101010053\",\"ocode\":\"91027843\"},{\"name\":\"Marincess Snow\",\"ucode\":\"101010067\",\"ocode\":\"80627281\"},{\"name\":\"Marincess Cascade\",\"ucode\":\"101010068\",\"ocode\":\"27012990\"},{\"name\":\"Hakai Douji Arha\",\"ucode\":\"101010008\",\"ocode\":\"26236560\"},{\"name\":\"Hakai Douji Rakia\",\"ucode\":\"101010009\",\"ocode\":\"53624265\"},{\"name\":\"Hakaishin no Magatama\",\"ucode\":\"101010010\",\"ocode\":\"89019964\"},{\"name\":\"Hakaishin Ragia\",\"ucode\":\"101010043\",\"ocode\":\"67680512\"},{\"name\":\"Hakaishin Arba\",\"ucode\":\"101010044\",\"ocode\":\"93084621\"},{\"name\":\"Hakai Souohshin Raigou\",\"ucode\":\"101010045\",\"ocode\":\"29479265\"},{\"name\":\"The Shackles of Souoh\",\"ucode\":\"101010054\",\"ocode\":\"27412542\"},{\"name\":\"Wail of the Hakaishin\",\"ucode\":\"101010055\",\"ocode\":\"54807656\"},{\"name\":\"Hakai Homily\",\"ucode\":\"101010069\",\"ocode\":\"53417695\"},{\"name\":\"Hakai Dual Dirge\",\"ucode\":\"101010070\",\"ocode\":\"80801743\"},{\"name\":\"Processlayer Sigma\",\"ucode\":\"100423001\",\"ocode\":\"27182739\"},{\"name\":\"Processlayer Nabla\",\"ucode\":\"100423002\",\"ocode\":\"53577438\"}]";
        }
    }
}

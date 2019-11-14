using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Converters;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServerTests.Services.Converters
{
    [TestFixture]
    class BetaCardToOfficialConverterTests
    {
        Mock<IAdminConfig> _adminConfigMock;
        Mock<ICardsDataDownloader> _downloaderMock;
        Mock<IYDKToDecklistConverter> _converterMock;
        
        static Archetype _defaultArchetype;
        static Card _pSYFrameDriver;
        private static Card _checksumDragon;
        private static Card _pankratops;
        private static Card _mokeyMokeyKing;
        private static Card _officialchecksumDragon;

        [SetUp]
        public void SetUp()
        {
            _downloaderMock = new Mock<ICardsDataDownloader>();
            _downloaderMock
                .Setup(x => x.DownloadCardsFromWebsite("http://eeriecode.altervista.org/tools/get_beta_cards.php"))
                .ReturnsAsync(_getValidCardsDataFromBetaApi());
            _adminConfigMock = new Mock<IAdminConfig>();
            _adminConfigMock.Setup(x => x.BetaToOfficialCardApiURL).Returns("http://eeriecode.altervista.org/tools/get_beta_cards.php");

            _createDefaultArchetype();
            _createPSYFrameDriver(_defaultArchetype);
            _createMokeyMokeyKing(_defaultArchetype);
            _createChecksumDragon(_defaultArchetype);
            _createPankratops(_defaultArchetype);
            _createOfficialChecksumDragon(_defaultArchetype);
            _converterMock = new Mock<IYDKToDecklistConverter>();
            _converterMock.Setup(x => x.Convert(It.IsAny<String>()))
                .Returns(new Decklist(new List<Card>() {_pankratops,_checksumDragon}, new List<Card>() {_mokeyMokeyKing }, new List<Card>() {_pSYFrameDriver }));

        }

        [Test]
        public async Task DeckContainsBetaCard_OfficialIDAppears_ConvertCardToOfficial()
        {
            using (var dbInMemory = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                dbInMemory.Database.EnsureCreated();
                _addPSYFrameDriver(dbInMemory);
                _addMokeyMokeyKing(dbInMemory);
                _addChecksumDragon(dbInMemory);
                _addPankratops(dbInMemory);
                var banlist = new Banlist("2019.10 TCG", 1);
                banlist.ForbiddenCards.Add(_checksumDragon);
                dbInMemory.Banlists.Add(banlist);
                await dbInMemory.SaveChangesAsync();
                var decklist = _converterMock.Object.Convert(GetProperYDKString());
                decklist.Name = "Lunalight";
                decklist.Archetype = _defaultArchetype;
                dbInMemory.Decklists.Add(decklist);

                await dbInMemory.SaveChangesAsync();
                IBetaCardToOfficialConverter _converter = new BetaCardToOfficialConverter(dbInMemory, _adminConfigMock.Object, _downloaderMock.Object);
                await _converter.UpdateCardsFromBetaToOfficial();
                var decklistFromDb = dbInMemory.Decklists.First();

                Assert.Multiple(() =>
                {
                    //tests if a beta card (Cheksum dragon with betaid:100336006) is converted to 94136469
                    Assert.IsNotNull(decklistFromDb.MainDeck.FirstOrDefault(x => x.PassCode == 94136469));
                    Assert.IsNull(decklistFromDb.MainDeck.FirstOrDefault(x => x.PassCode == 100336006));
                });
            }
        }

        [Test]
        public async Task DeckContainsBetaCard_CardWithOfficialIdExistsInDB_MergeCardWithOfficial()
        {
            using (var dbInMemory = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                dbInMemory.Database.EnsureCreated();
                _addPSYFrameDriver(dbInMemory);
                _addMokeyMokeyKing(dbInMemory);
                _addChecksumDragon(dbInMemory);
                _addPankratops(dbInMemory);
                _addOfficialChecksumDragon(dbInMemory);
                var banlist = new Banlist("2019.10 TCG", 1);
                banlist.ForbiddenCards.Add(_checksumDragon);
                dbInMemory.Banlists.Add(banlist);
                await dbInMemory.SaveChangesAsync();
                var decklist = _converterMock.Object.Convert(GetProperYDKString());
                decklist.Name = "Lunalight";
                decklist.Archetype = _defaultArchetype;
                dbInMemory.Decklists.Add(decklist);

                await dbInMemory.SaveChangesAsync();
                IBetaCardToOfficialConverter _converter = new BetaCardToOfficialConverter(dbInMemory, _adminConfigMock.Object, _downloaderMock.Object);
                await _converter.UpdateCardsFromBetaToOfficial();
                var decklistFromDb = dbInMemory.Decklists.First();

                Assert.Multiple(() =>
                {
                    //tests if a beta card (Cheksum dragon with betaid:100336006) is converted to 94136469
                    Assert.IsNotNull(decklistFromDb.MainDeck.FirstOrDefault(x => x.PassCode == 94136469));
                    Assert.IsNull(decklistFromDb.MainDeck.FirstOrDefault(x => x.PassCode == 100336006));
                    Assert.IsNull(dbInMemory.Cards.FirstOrDefault(x => x.PassCode == 100336006));
                    Assert.AreEqual(1,dbInMemory.Cards.FirstOrDefault(x => x.PassCode == 94136469).DecksWhereThisCardIsInMainDeck.Count);
                    Assert.IsTrue(dbInMemory.Cards.FirstOrDefault(x => x.PassCode == 94136469).BanlistsWhereThisCardIsForbidden.Contains(banlist));

                });
            }
        }

        private static void _createDefaultArchetype()
        {
            _defaultArchetype = new Archetype(Archetype.Default, true);
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

        private static void _addPSYFrameDriver(YgoProAnalyticsDatabase db)
        {
            db.Cards.Add(_pSYFrameDriver);
        }

        private static void _createPSYFrameDriver(Archetype archetype)
        {
            _pSYFrameDriver = Card.Create(
                                49036338,
                                "PSY-Frame Driver",
                                "Description: PSY-Frame Driver",
                                "Monster Card",
                                "normal",
                                null,
                                null,
                                archetype
                            );
        }

        private static void _addChecksumDragon(YgoProAnalyticsDatabase db)
        {
            db.Cards.Add(_checksumDragon);
        }

        private static void _addOfficialChecksumDragon(YgoProAnalyticsDatabase db)
        {
            db.Cards.Add(_officialchecksumDragon);
        }

        private static void _createChecksumDragon(Archetype archetype)
        {
            _checksumDragon = Card.Create(
                                100336006,
                                "Checksum Dragon",
                                "Description: Checksum Dragon",
                                "Monster Card",
                                "normal",
                                "someUrl.com/100336006",
                                "someOtherUrl.com/100336006",
                                archetype
                            );
        }
        private static void _createOfficialChecksumDragon(Archetype archetype)
        {
            _officialchecksumDragon = Card.Create(
                                94136469,
                                "Checksum Dragon",
                                "Description: Checksum Dragon",
                                "Monster Card",
                                "normal",
                                null,
                                null,
                                archetype
                            );
        }

        private static void _addMokeyMokeyKing(YgoProAnalyticsDatabase db)
        {
            db.Cards.Add(
                _mokeyMokeyKing
            );
        }

        private static void _createMokeyMokeyKing(Archetype archetype)
        {
            _mokeyMokeyKing = Card.Create(
                                13803864,
                                "Mokey Mokey King",
                                "Description: Mokey Mokey King",
                                "Fusion Monster Card",
                                "normal",
                                null,
                                null,
                                archetype
                            );
        }

        private static void _addPankratops(YgoProAnalyticsDatabase db)
        {
            db.Cards.Add(_pankratops);
        }

        private static void _createPankratops(Archetype archetype)
        {
            _pankratops = Card.Create(
                                82385847,
                                "Dinowrestler Pankratops",
                                "Description: Dinowrestler Pankratops",
                                "Effect Monster Card",
                                "normal",
                                null,
                                null,
                                archetype
                            );
        }

        private string GetProperYDKString()
        {
            return "#created by ygopro2\r\n#main\r\n100336006\r\n59509952\r\n80701178\r\n80701178\r\n18474999\r\n95492061\r\n53303460\r\n53303460\r\n90307777\r\n82085295\r\n82085295\r\n16229315\r\n16229315\r\n49036338\r\n52068432\r\n25857246\r\n4810828\r\n65877963\r\n65877963\r\n65877963\r\n26674724\r\n26674724\r\n26674724\r\n77235086\r\n77235086\r\n89463537\r\n99185129\r\n99185129\r\n38814750\r\n38814750\r\n38814750\r\n96729612\r\n96729612\r\n96729612\r\n32807846\r\n51124303\r\n51124303\r\n14735698\r\n86758915\r\n97211663\r\n97211663\r\n24224830\r\n24224830\r\n24224830\r\n#extra\r\n13803864\r\n80532587\r\n80773359\r\n41517789\r\n74586817\r\n79606837\r\n79606837\r\n79606837\r\n55863245\r\n57707471\r\n6983839\r\n46772449\r\n8809344\r\n87054946\r\n2857636\r\n!side\r\n10000080\r\n10000080\r\n10000080\r\n82385847\r\n82385847\r\n13974207\r\n13974207\r\n13974207\r\n52738610\r\n43898403\r\n43898403\r\n43898403\r\n15693423\r\n15693423\r\n15693423";
        }
        private string _getValidCardsDataFromBetaApi()
        {
            return "[{\"name\":\"Flash Charge Dragon\",\"ucode\":\"0\",\"ocode\":\"95372220\"},{\"name\":\"Mischief of the Time Goddess\",\"ucode\":\"0\",\"ocode\":\"92182447\"},{\"name\":\"Clock Lizard\",\"ucode\":\"0\",\"ocode\":\"51476410\"},{\"name\":\"Borrel Supplier\",\"ucode\":\"100336023\",\"ocode\":\"30131474\"},{\"name\":\"Checksum Dragon\",\"ucode\":\"100336006\",\"ocode\":\"94136469\"},{\"name\":\"Absorouter Dragon\",\"ucode\":\"100336005\",\"ocode\":\"67748760\"},{\"name\":\"Exploderokket Dragon\",\"ucode\":\"100336004\",\"ocode\":\"31353051\"},{\"name\":\"Salamangreat Pyro Phoenix\",\"ucode\":\"101010039\",\"ocode\":\"31313405\"},{\"name\":\"Borreload Furious Dragon\",\"ucode\":\"100336051\",\"ocode\":\"92892239\"},{\"name\":\"Sawnborrel Dragon\",\"ucode\":\"100336052\",\"ocode\":\"29296344\"},{\"name\":\"Rapid Trigger\",\"ucode\":\"100336024\",\"ocode\":\"67526112\"},{\"name\":\"Topologic Zerovoros\",\"ucode\":\"100336041\",\"ocode\":\"66403530\"},{\"name\":\"Firewall Dragon Darkfluid\",\"ucode\":\"101010037\",\"ocode\":\"68934651\"},{\"name\":\"Starliege Dragon Seyfert\",\"ucode\":\"101010014\",\"ocode\":\"15381421\"},{\"name\":\"Galaxy Satellite Dragon\",\"ucode\":\"101010047\",\"ocode\":\"92362073\"},{\"name\":\"True Exodia\",\"ucode\":\"100250001\",\"ocode\":\"0\"},{\"name\":\"Seraphim Papillon\",\"ucode\":\"101010050\",\"ocode\":\"91140491\"},{\"name\":\"Silverrokket Dragon\",\"ucode\":\"100336001\",\"ocode\":\"32476603\"},{\"name\":\"Tenyi Dragon Ajna\",\"ucode\":\"101010019\",\"ocode\":\"87052196\"},{\"name\":\"Dragon Ogre Deity of Tenyi\",\"ucode\":\"101010035\",\"ocode\":\"5041348\"},{\"name\":\"Rokket Tracer\",\"ucode\":\"100336002\",\"ocode\":\"68464358\"},{\"name\":\"Rokket Recharger\",\"ucode\":\"100336003\",\"ocode\":\"5969957\"},{\"name\":\"Striker Dragon\",\"ucode\":\"100200165\",\"ocode\":\"73539069\"},{\"name\":\"Kongrade, Primate of Conglomerates\",\"ucode\":\"101010024\",\"ocode\":\"49729312\"},{\"name\":\"Granite Loyalist\",\"ucode\":\"101010036\",\"ocode\":\"32530043\"},{\"name\":\"Zero Day Blaster\",\"ucode\":\"100336033\",\"ocode\":\"93014827\"},{\"name\":\"Gunslinger Execution\",\"ucode\":\"100336034\",\"ocode\":\"20419926\"},{\"name\":\"Marincess Mandarin\",\"ucode\":\"101010002\",\"ocode\":\"28174796\"},{\"name\":\"Marincess Crown Tail\",\"ucode\":\"101010003\",\"ocode\":\"54569495\"},{\"name\":\"Marincess Blue Tang\",\"ucode\":\"101010004\",\"ocode\":\"91953000\"},{\"name\":\"Marincess Crystal Heart\",\"ucode\":\"101010040\",\"ocode\":\"67712104\"},{\"name\":\"Marincess Wonder Heart\",\"ucode\":\"101010041\",\"ocode\":\"94207108\"},{\"name\":\"Marincess Sea Angel\",\"ucode\":\"101010042\",\"ocode\":\"30691817\"},{\"name\":\"Marincess Battle Ocean\",\"ucode\":\"101010053\",\"ocode\":\"91027843\"},{\"name\":\"Marincess Snow\",\"ucode\":\"101010067\",\"ocode\":\"80627281\"},{\"name\":\"Marincess Cascade\",\"ucode\":\"101010068\",\"ocode\":\"27012990\"},{\"name\":\"Hakai Douji Arha\",\"ucode\":\"101010008\",\"ocode\":\"26236560\"},{\"name\":\"Hakai Douji Rakia\",\"ucode\":\"101010009\",\"ocode\":\"53624265\"},{\"name\":\"Hakaishin no Magatama\",\"ucode\":\"101010010\",\"ocode\":\"89019964\"},{\"name\":\"Hakaishin Ragia\",\"ucode\":\"101010043\",\"ocode\":\"67680512\"},{\"name\":\"Hakaishin Arba\",\"ucode\":\"101010044\",\"ocode\":\"93084621\"},{\"name\":\"Hakai Souohshin Raigou\",\"ucode\":\"101010045\",\"ocode\":\"29479265\"},{\"name\":\"The Shackles of Souoh\",\"ucode\":\"101010054\",\"ocode\":\"27412542\"},{\"name\":\"Wail of the Hakaishin\",\"ucode\":\"101010055\",\"ocode\":\"54807656\"},{\"name\":\"Hakai Homily\",\"ucode\":\"101010069\",\"ocode\":\"53417695\"},{\"name\":\"Hakai Dual Dirge\",\"ucode\":\"101010070\",\"ocode\":\"80801743\"},{\"name\":\"Processlayer Sigma\",\"ucode\":\"100423001\",\"ocode\":\"27182739\"},{\"name\":\"Processlayer Nabla\",\"ucode\":\"100423002\",\"ocode\":\"53577438\"}]";
        }
    }
}

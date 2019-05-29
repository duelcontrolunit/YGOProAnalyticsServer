using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Others;
using YGOProAnalyticsServerTests.TestingHelpers;
using Moq;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServerTests.Others
{
    [TestFixture]
    class DecklistServiceTests
    {
        DecklistService _decklistService;
        YgoProAnalyticsDatabase _db;
        readonly CardsAndDecksHelper _helper = new CardsAndDecksHelper();
        Mock<IBanlistService> _banlistServiceMock;

        [SetUp]
        public void SetUp()
        {
            _db = new YgoProAnalyticsDatabase(SqlInMemoryHelper.SqlLiteOptions<YgoProAnalyticsDatabase>());
            _db.Database.EnsureCreated();
            _banlistServiceMock = new Mock<IBanlistService>();
        }

        private void _initService()
        {
            _decklistService = new DecklistService(_db, _banlistServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        [Test]
        public async Task GetByIdWithAllDataIncluded_ThereIsNoDecklistWithGivenId_WeGetNull()
        {
            _initService();
            Assert.IsNull(await _decklistService.GetByIdWithAllDataIncluded(1));
        }

        [Test]
        public async Task GetByIdWithAllDataIncluded_ThereIsDecklistWithGivenId_WeGetDecklistWithAllDatas()
        {
            var banlist = new Banlist("2019.11 TCG", 1);
            var archetype = new Archetype("Valid archetype", false);

            var bannedPendulumMonster = _helper.GetCard(archetype);
            bannedPendulumMonster.MonsterCard = _helper.GetMonsterCard(bannedPendulumMonster);
            bannedPendulumMonster.MonsterCard.PendulumMonsterCard = _helper.GetPendulumMonsterCard(bannedPendulumMonster.MonsterCard);
            banlist.ForbiddenCards.Add(bannedPendulumMonster);

            var limitedLinkMonster = _helper.GetCard(archetype);
            limitedLinkMonster.MonsterCard = _helper.GetMonsterCard(limitedLinkMonster);
            limitedLinkMonster.MonsterCard.LinkMonsterCard = _helper.GetLinkMonsterCard(limitedLinkMonster.MonsterCard);
            banlist.LimitedCards.Add(limitedLinkMonster);

            var decklist = _helper.GetValidDecklistWithStatistics(archetype);
            decklist.MainDeck.Add(bannedPendulumMonster);
            decklist.ExtraDeck.Add(limitedLinkMonster);

            _db.Archetypes.Add(archetype);
            _db.Banlists.Add(banlist);
            _db.Cards.Add(bannedPendulumMonster);
            _db.Cards.Add(limitedLinkMonster);
            _db.Decklists.Add(decklist);

            await _db.SaveChangesAsync();
            _initService();

            var decklistFromDb = await _decklistService.GetByIdWithAllDataIncluded(1);

            Assert.Multiple(()=> {
                Assert.NotZero(decklistFromDb.Archetype.Name.Length);
                Assert.NotZero(decklistFromDb.DecklistStatistics.Count);
                Assert.NotZero(decklistFromDb.DecklistStatistics.Count);
                Assert.NotNull(decklistFromDb.MainDeck.FirstOrDefault()?.Archetype);
                Assert.NotNull(decklistFromDb.MainDeck.FirstOrDefault()?.MonsterCard.PendulumMonsterCard);
                Assert.NotNull(decklistFromDb.ExtraDeck.FirstOrDefault()?.MonsterCard.LinkMonsterCard);
                Assert.NotZero(decklistFromDb.MainDeck.FirstOrDefault().BanlistsWhereThisCardIsForbidden.Count);
                Assert.NotZero(decklistFromDb.ExtraDeck.FirstOrDefault().BanlistsWhereThisCardIsLimited.Count);
            });

            Assert.IsNotNull("");
        }
    }
}

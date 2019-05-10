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

namespace YGOProAnalyticsServerTests.Others
{
    [TestFixture]
    class DecklistServiceTests
    {
        DecklistService _decklistService;
        YgoProAnalyticsDatabase _db;

        [SetUp]
        public void SetUp()
        {
            _db = new YgoProAnalyticsDatabase(SqlInMemoryHelper.SqlLiteOptions<YgoProAnalyticsDatabase>());
            _db.Database.EnsureCreated();
        }

        private void _initService()
        {
            _decklistService = new DecklistService(_db);
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

            var bannedPendulumMonster = _getCard(archetype);
            bannedPendulumMonster.MonsterCard = _getMonsterCard(bannedPendulumMonster);
            bannedPendulumMonster.MonsterCard.PendulumMonsterCard = _getPendulumMonsterCard(bannedPendulumMonster.MonsterCard);
            banlist.ForbiddenCards.Add(bannedPendulumMonster);

            var limitedLinkMonster = _getCard(archetype);
            limitedLinkMonster.MonsterCard = _getMonsterCard(limitedLinkMonster);
            limitedLinkMonster.MonsterCard.LinkMonsterCard = _getLinkMonsterCard(limitedLinkMonster.MonsterCard);
            banlist.LimitedCards.Add(limitedLinkMonster);

            var decklist = _getValidDecklistWithStatistics(archetype);
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

        private Decklist _getValidDecklistWithStatistics(Archetype archetype)
        {
            var decklist = new Decklist("Valid decklist", archetype, new DateTime(1997, 4, 29));
            decklist.DecklistStatistics.Add(_getValidStatisticsForValidDeck());

            return decklist;
        }

        private PendulumMonsterCard _getPendulumMonsterCard(MonsterCard monster)
        {
            return PendulumMonsterCard.Create(5, monster);
        }

        private LinkMonsterCard _getLinkMonsterCard(MonsterCard monster)
        {
            return LinkMonsterCard.Create(1, topLeftLinkMarker: true, false, false, false, false, false, false, false, monster);
        }

        private Card _getCard(Archetype archetype)
        {
            return Card.Create(
                passCode: 12345,
                name: "monster name",
                description: "monster desc",
                type: "monster Type",
                race: "monster race",
                imageUrl: "imgUrl",
                smallImageUrl: "smallImgUrl",
                archetype
            );
        }

        private MonsterCard _getMonsterCard(Card card)
        {
            return MonsterCard.Create(
                attack: "1234",
                defence: "?",
                levelOrRank: 5,
                attribute: "dark",
                card: card
            );
        }

        private DecklistStatistics _getValidStatisticsForValidDeck()
        {
            DecklistStatistics statistics = new DecklistStatistics();
            statistics.IncrementNumberOfTimesWhenDeckWasUsed();

            statistics.GetType()
                .GetProperty(nameof(DecklistStatistics.DateWhenDeckWasUsed))
                .SetValue(statistics, new DateTime(1997, 4, 29));

            statistics.GetType()
               .GetProperty(nameof(DecklistStatistics.Id))
               .SetValue(statistics, 1);

            return statistics;
        }
    }
}

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
using YGOProAnalyticsServer.Exceptions;
using YGOProAnalyticsServer.Services.Analyzers;
using YGOProAnalyticsServer.Services.Analyzers.Interfaces;

namespace YGOProAnalyticsServerTests.Services.Analyzers
{
    [TestFixture]
    class ArchetypeOfDecklistAnalyzerTests
    {
        IArchetypeAndDecklistAnalyzer _analyzer;
        Card nekrozOfBrionac;
        Card nekrozMirror;
        Card herald;
        Card bookstone;
        [Test]
        public async Task GetArchetypeOfDecklist_DeckHas1MainArchetype_DeckGets1Archetype()
        {
            using (var dbInMemory = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                dbInMemory.Database.EnsureCreated();
                var archetypeNekroz = new Archetype("Nekroz", true);
                var archetypeHerald = new Archetype("Herald", true);
                nekrozOfBrionac = _NekrozofBrionac(archetypeNekroz);
                nekrozMirror = _NekrozMirror(archetypeNekroz);
                herald = _Herald(archetypeHerald);
                dbInMemory.Cards.AddRange(nekrozOfBrionac, nekrozMirror, herald);
                await dbInMemory.SaveChangesAsync();
                var decklist = new Decklist(new List<Card> { nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozMirror }, new List<Card> { herald, herald }, new List<Card> { herald });

                _analyzer = new ArchetypeAndDecklistAnalyzer(dbInMemory);
                _analyzer.GetArchetypeOfTheDecklistWithStatistics(decklist, DateTime.Now);

                Assert.AreEqual("Nekroz", decklist.Archetype.Name);
            }
        }

        [Test]
        public async Task GetArchetypeOfDecklist_DeckHas2MainArchetypes_DeckGetsNotPureArchetype()
        {
            using (var dbInMemory = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                dbInMemory.Database.EnsureCreated();
                var archetypeNekroz = new Archetype("Nekroz", true);
                var archetypeHerald = new Archetype("Herald", true);
                var archetypeImpcantation = new Archetype("Impcantation", true);
                nekrozOfBrionac = _NekrozofBrionac(archetypeNekroz);
                nekrozMirror = _NekrozMirror(archetypeNekroz);
                herald = _Herald(archetypeHerald);
                bookstone = _ImpcantationBookStone(archetypeImpcantation);
                dbInMemory.Cards.AddRange(nekrozOfBrionac, nekrozMirror, herald, bookstone);
                await dbInMemory.SaveChangesAsync();
                var decklist = new Decklist(new List<Card> { nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, bookstone, bookstone, bookstone, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, bookstone, bookstone, bookstone }, new List<Card> { herald, herald }, new List<Card>());

                _analyzer = new ArchetypeAndDecklistAnalyzer(dbInMemory);
                _analyzer.GetArchetypeOfTheDecklistWithStatistics(decklist, DateTime.Now);

                Assert.IsFalse(decklist.Archetype.IsPureArchetype);
            }
        }

        [Test]
        public async Task GetArchetypeOfDecklist_DeckHasDefaultArchetype_DeckGetsDefaultArchetype()
        {
            using (var dbInMemory = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                dbInMemory.Database.EnsureCreated();
                var archetype = new Archetype(Archetype.Default, true);
                nekrozOfBrionac = _NekrozofBrionac(archetype);
                nekrozMirror = _NekrozMirror(archetype);
                herald = _Herald(archetype);
                bookstone = _ImpcantationBookStone(archetype);
                dbInMemory.Cards.AddRange(nekrozOfBrionac, nekrozMirror, herald, bookstone);
                await dbInMemory.SaveChangesAsync();
                var decklist = new Decklist(new List<Card> { nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, bookstone, bookstone, bookstone }, new List<Card> { herald, herald }, new List<Card>());

                _analyzer = new ArchetypeAndDecklistAnalyzer(dbInMemory);
                _analyzer.GetArchetypeOfTheDecklistWithStatistics(decklist, DateTime.Now);

                Assert.AreEqual(Archetype.Default, decklist.Archetype.Name);
            }
        }

        [Test]
        public async Task GetArchetypeOfDecklist_DeckHasDefaultArchetypeButManyCardsFromTheSameRace_DeckGetsRaceArchetype()
        {
            using (var dbInMemory = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                dbInMemory.Database.EnsureCreated();
                var archetype = new Archetype(Archetype.Default, true);
                nekrozOfBrionac = _NekrozofBrionac(archetype);
                nekrozMirror = _NekrozMirror(archetype);
                herald = _Herald(archetype);
                bookstone = _ImpcantationBookStone(archetype);
                dbInMemory.Cards.AddRange(nekrozOfBrionac, nekrozMirror, herald, bookstone);
                await dbInMemory.SaveChangesAsync();
                var decklist = new Decklist(new List<Card> { nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozMirror, nekrozMirror, nekrozMirror }, new List<Card> { herald }, new List<Card> { nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac, nekrozOfBrionac });

                _analyzer = new ArchetypeAndDecklistAnalyzer(dbInMemory);
                _analyzer.GetArchetypeOfTheDecklistWithStatistics(decklist, DateTime.Now);

                Assert.AreEqual(nekrozOfBrionac.Race+"-Type", decklist.Archetype.Name);
            }
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

        private static Card _ImpcantationBookStone(Archetype archetype)
        {
            var card = Card.Create(
                 18474999,
                 "Impcantation Bookstone",
                 "Description: Impcantation Bookstone",
                 "Monster Card",
                 "Spellcaster",
                 null,
                 null,
                 archetype
             );
            card.MonsterCard = MonsterCard.Create("0", "0", 5, "Dark", card);
            return card;
        }

        private static Card _NekrozofBrionac(Archetype archetype)
        {
            var card = Card.Create(
                 26674724,
                 "Nekroz of Brionac",
                 "Description: Nekroz of Brionac",
                 "Monster Card",
                 "Warrior",
                 null,
                 null,
                 archetype
             );
            card.MonsterCard = MonsterCard.Create("2300", "1200", 6, "Water", card);
            return card;
        }
        private static Card _NekrozMirror(Archetype archetype)
        {
            var card = Card.Create(
                 14735698,
                 "Nekroz Mirror",
                 "Description: Nekroz Mirror",
                 "Ritual Spell Card",
                 "Spell",
                 null,
                 null,
                 archetype
         );
            return card;
        }
        private static Card _Herald(Archetype archetype)
        {
            var card = Card.Create(
                 79606837,
                 "Herald of the Arc Light",
                 "Description: Herald of the Arc Light",
                 "Effect Synchro Monster Card",
                 "Fairy",
                 null,
                 null,
                 archetype
         );
            card.MonsterCard = MonsterCard.Create("600", "1000", 4, "Light", card);
            return card;
        }
    }
}

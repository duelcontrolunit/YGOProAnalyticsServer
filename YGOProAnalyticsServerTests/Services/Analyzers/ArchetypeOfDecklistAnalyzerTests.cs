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


        [Test]
        public async Task SetDecklistArchetypeFromArchetypeCardsUsedInIt_ProperDecklistWithDominantArchetype_GetDecklistWithArchetypeAsync()
        {
            using (var dbInMemory = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                dbInMemory.Database.EnsureCreated();
                _analyzer = new ArchetypeAndDecklistAnalyzer(dbInMemory);
                var nekrozArchetype = new Archetype("Nekroz", true);
                var heraldArchetype = new Archetype("Herald", true);
                await dbInMemory.SaveChangesAsync();
                var decklist = new Decklist(new List<Card> { _NekrozofBrionac(nekrozArchetype), _NekrozMirror(nekrozArchetype) }, new List<Card> { _Herald(heraldArchetype) }, new List<Card>());
                var resultDecklist = await _analyzer.SetDecklistArchetypeFromArchetypeCardsUsedInIt(decklist);
                Assert.AreEqual("Nekroz", resultDecklist.Archetype.Name);
            }
        }
        [Test]
        public async Task SetDecklistArchetypeFromArchetypeCardsUsedInIt_ProperDecklistWithMultiArchetype_GetDecklistWithArchetypeAsync()
        {
            using (var dbInMemory = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                dbInMemory.Database.EnsureCreated();
                _analyzer = new ArchetypeAndDecklistAnalyzer(dbInMemory);
                var nekrozArchetype = new Archetype("Nekroz", true);
                var heraldArchetype = new Archetype("Herald", true);
                await dbInMemory.SaveChangesAsync();
                var decklist = new Decklist(new List<Card> { _NekrozofBrionac(nekrozArchetype), _NekrozMirror(new Archetype(Archetype.Default,true)) }, new List<Card> { _Herald(heraldArchetype) }, new List<Card>());
                var resultDecklist = await _analyzer.SetDecklistArchetypeFromArchetypeCardsUsedInIt(decklist);
                Assert.AreEqual("Nekroz Herald", resultDecklist.Archetype.Name);
            }
        }

        [Test]
        public async Task SetDecklistArchetypeFromArchetypeCardsUsedInIt_ProperDecklistWithNeutralArchetype_GetDecklistWithDefaultArchetypeAsync()
        {
            using (var dbInMemory = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                dbInMemory.Database.EnsureCreated();
                _analyzer = new ArchetypeAndDecklistAnalyzer(dbInMemory);

                await dbInMemory.SaveChangesAsync();
                var decklist = new Decklist(new List<Card> { _NekrozofBrionac(new Archetype(Archetype.Default, true)), _NekrozMirror(new Archetype(Archetype.Default, true)) }, new List<Card> { _Herald(new Archetype(Archetype.Default, true)) }, new List<Card>());
                var resultDecklist = await _analyzer.SetDecklistArchetypeFromArchetypeCardsUsedInIt(decklist);
                Assert.AreEqual(Archetype.Default, resultDecklist.Archetype.Name);
            }
        }

        [Test]
        public async Task SetDecklistArchetypeFromArchetypeCardsUsedInIt_DecklistWithNoCards_ThrowsEmptyDecklistException()
        {
            using (var dbInMemory = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                dbInMemory.Database.EnsureCreated();
                _analyzer = new ArchetypeAndDecklistAnalyzer(dbInMemory);

                await dbInMemory.SaveChangesAsync();
                var decklist = new Decklist(new List<Card>(), new List<Card>(), new List<Card>());

                Assert.ThrowsAsync<EmptyDecklistException>(() => _analyzer.SetDecklistArchetypeFromArchetypeCardsUsedInIt(decklist));
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
        private static Card _NekrozofBrionac(Archetype archetype)
        {
            return Card.Create(
                 26674724,
                 "Nekroz of Brionac",
                 "Description: Nekroz of Brionac",
                 "Monster Card",
                 "normal",
                 null,
                 null,
                 archetype
             );
        }
        private static Card _NekrozMirror(Archetype archetype)
        {
            return Card.Create(
                14735698,
                "Nekroz Mirror",
                "Description: Nekroz Mirror",
                "Ritual Spell Card",
                "spell",
                null,
                null,
                archetype
        );
        }
        private static Card _Herald(Archetype archetype)
        {
            return Card.Create(
                 79606837,
                 "Herald of the Arc Light",
                 "Description: Herald of the Arc Light",
                 "Effect Synchro Monster Card",
                 "",
                 null,
                 null,
                 archetype
         );
        }
    }
}

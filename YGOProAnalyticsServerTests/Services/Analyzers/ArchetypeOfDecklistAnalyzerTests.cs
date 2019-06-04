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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer.Services.Converters;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using Moq;
using YGOProAnalyticsServer.Database;
using System.Linq;
using YGOProAnalyticsServer.DbModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Threading.Tasks;

namespace YGOProAnalyticsServerTests.Services.Converters
{
    [TestFixture]
    class YDKToDecklistConverterTests
    {
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
                _addPankratops(dbInMemory, archetype);
                await dbInMemory.SaveChangesAsync();
                _converter = new YDKToDecklistConverter(dbInMemory);
                var decklist = _converter.Convert(GetProperYDKString());
                Assert.Greater(decklist.MainDeck.Count, 0);
                Assert.Greater(decklist.ExtraDeck.Count, 0);
                Assert.Greater(decklist.SideDeck.Count, 1);
            }
        }
        private string GetProperYDKString()
        {
            return "#created by ygopro2\r\n#main\r\n49036338\r\n59509952\r\n80701178\r\n80701178\r\n18474999\r\n95492061\r\n53303460\r\n53303460\r\n90307777\r\n82085295\r\n82085295\r\n16229315\r\n16229315\r\n16229315\r\n52068432\r\n25857246\r\n4810828\r\n65877963\r\n65877963\r\n65877963\r\n26674724\r\n26674724\r\n26674724\r\n77235086\r\n77235086\r\n89463537\r\n99185129\r\n99185129\r\n38814750\r\n38814750\r\n38814750\r\n96729612\r\n96729612\r\n96729612\r\n32807846\r\n51124303\r\n51124303\r\n14735698\r\n86758915\r\n97211663\r\n97211663\r\n24224830\r\n24224830\r\n24224830\r\n#extra\r\n13803864\r\n80532587\r\n80773359\r\n41517789\r\n74586817\r\n79606837\r\n79606837\r\n79606837\r\n55863245\r\n57707471\r\n6983839\r\n46772449\r\n8809344\r\n87054946\r\n2857636\r\n!side\r\n10000080\r\n10000080\r\n10000080\r\n82385847\r\n82385847\r\n13974207\r\n13974207\r\n13974207\r\n52738610\r\n43898403\r\n43898403\r\n43898403\r\n15693423\r\n15693423\r\n15693423";
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
    }
}

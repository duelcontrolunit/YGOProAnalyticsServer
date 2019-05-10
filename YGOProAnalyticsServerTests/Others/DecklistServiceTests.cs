using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer.Database;
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
    }
}

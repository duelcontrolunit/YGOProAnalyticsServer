using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Updaters;
using YGOProAnalyticsServer.Services.Downloaders;
using YGOProAnalyticsServer.Services.Builders;
using System.Threading.Tasks;
using YGOProAnalyticsServer;

namespace YGOProAnalyticsServerTests.Database
{
    [TestFixture]
    class YgoProAnalyticsDatabaseTests
    {
        IAdminConfig _adminConfig;
        [SetUp]
        public void SetUp()
        {
            _adminConfig = new AdminConfig();
        }
        [Test]
        public void DatabaseExists_ReturnsTrue()
        {
            var optionsBuilder = new DbContextOptionsBuilder<YgoProAnalyticsDatabase>();
            optionsBuilder.UseSqlServer(YgoProAnalyticsDatabase.ConnectionString(_adminConfig.DBUser, _adminConfig.DBPassword));
            using (YgoProAnalyticsDatabase db = new YgoProAnalyticsDatabase(optionsBuilder.Options))
            {
                Assert.IsTrue((db.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists());
            }
        }
    }
}

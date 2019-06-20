using NUnit.Framework;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.Models;
using YGOProAnalyticsServer.Services.Updaters;
using YGOProAnalyticsServer.Services.Updaters.Interfaces;
using YGOProAnalyticsServerTests.TestingHelpers;

namespace YGOProAnalyticsServerTests.Services.Updater
{
    [TestFixture]
    class ServerActivityUpdaterTests
    {
        IServerActivityUpdater _updater;
        YgoProAnalyticsDatabase _dbInMemory;

        [SetUp]
        public async Task SetUp()
        {
            _dbInMemory = new YgoProAnalyticsDatabase(SqlInMemoryHelper.SqlLiteOptions<YgoProAnalyticsDatabase>());
            await _dbInMemory.Database.EnsureCreatedAsync();
            _updater = new ServerActivityUpdater(_dbInMemory);
        }

        [TearDown]
        public void TearDown()
        {
            _dbInMemory.Dispose();
        }

        [Test]
        public async Task UpdateWithoutSavingChanges_ServerActivityStatisticsSuccesfullyUpdated()
        {
            var duelLogs = new List<DuelLog>();
            duelLogs.Add(new DuelLog(new DateTime(2018, 4, 29), new DateTime(2018, 4, 29), 1, 1, "test", "filename.yrp"));
            duelLogs.Add(new DuelLog(new DateTime(2018, 5, 2), new DateTime(2018, 5, 2), 1, 1, "test", "filename.yrp"));
            duelLogs.Add(new DuelLog(new DateTime(2018, 5, 2), new DateTime(2018, 5, 2), 1, 1, "test", "filename.yrp"));
            duelLogs.Add(new DuelLog(new DateTime(2018, 5, 2), new DateTime(2018, 5, 2), 1, 1, "test", "filename.yrp"));
            duelLogs.Add(new DuelLog(new DateTime(2018, 5, 1), new DateTime(2018, 5, 1), 1, 1, "test", "filename.yrp"));
            duelLogs.Add(new DuelLog(new DateTime(2018, 5, 1), new DateTime(2018, 5, 1), 1, 1, "test", "filename.yrp"));

            await _updater.UpdateWithoutSavingChanges(duelLogs);
            await _dbInMemory.SaveChangesAsync();

            var date_2018_5_1_Statistics = _dbInMemory.ServerActivityStatistics
                .Where(x => x.FromDate.Date == new DateTime(2018, 5, 1))
                .FirstOrDefault();
            var date_2018_5_2_Statistics = _dbInMemory.ServerActivityStatistics
                .Where(x => x.FromDate.Date == new DateTime(2018, 5, 2))
                .FirstOrDefault();
            var date_2018_4_29_Statistics = _dbInMemory.ServerActivityStatistics
                .Where(x => x.FromDate.Date == new DateTime(2018, 4, 29))
                .FirstOrDefault();

            Assert.Multiple(()=> {
                Assert.IsTrue(date_2018_5_1_Statistics?.NumberOfGames == 2);
                Assert.IsTrue(date_2018_5_2_Statistics?.NumberOfGames == 3);
                Assert.IsTrue(date_2018_4_29_Statistics?.NumberOfGames == 1);
            });
        }
    }
}

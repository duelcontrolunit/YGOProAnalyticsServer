using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Models;
using YGOProAnalyticsServer.Services.Analyzers;
using YGOProAnalyticsServerTests.TestingHelpers;

namespace YGOProAnalyticsServerTests.Services.Analyzers
{
    [TestFixture]
    class ServerActivityAnalyzerTests
    {
        ServerActivityAnalyzer _analyzer;
        YgoProAnalyticsDatabase _db;

        [SetUp]
        public async Task SetUp()
        {
            _db = new YgoProAnalyticsDatabase(SqlInMemoryHelper.SqlLiteOptions<YgoProAnalyticsDatabase>());
            await _db.Database.EnsureCreatedAsync();
            _analyzer = new ServerActivityAnalyzer(_db);
        }

        [Test]
        public void NumberOfGamesFromOneDay_WeGet3DuelLogs_Returned3()
        {
            var duelLog = new DuelLog(DateTime.Now, 1, 1, "", "");

            int result = _analyzer
                .NumberOfGamesFromOneDay(new List<DuelLog>() { duelLog, duelLog, duelLog });

            Assert.AreEqual(3, result);
        }

        [Test]
        public async Task NumberOfGamesFromOneDayAsync_ThereWas5GamesThatDay_WeGet5()
        {
            DateTime date = new DateTime(1997, 04, 29);
            var statistics = new ServerActivityStatistics(date) {
                NumberOfGames = 5
            };
            _db.ServerActivityStatistics.Add(statistics);
            await _db.SaveChangesAsync();

            Assert.AreEqual(5, await _analyzer.NumberOfGamesFromOneDayAsync(date));
        }

        [Test]
        public async Task NumberOfGamesFromOneDayAsync_WeHaveNotDataAboutThatDay_WeGet0()
        {
            Assert.AreEqual(0, await _analyzer.NumberOfGamesFromOneDayAsync(new DateTime(2002, 2, 2)));
        }

        [Test]
        public async Task NumberOfGamesFromMonthAsync_Was5Games_WeGet5()
        {
            DateTime date = new DateTime(2019, 04, 1);
            var statistics = new ServerActivityStatistics(date)
            {
                NumberOfGames = 2
            };

            DateTime date2 = new DateTime(2019, 04, 29);
            var statistics2 = new ServerActivityStatistics(date)
            {
                NumberOfGames = 3
            };

            _db.ServerActivityStatistics.Add(statistics);
            _db.ServerActivityStatistics.Add(statistics2);
            await _db.SaveChangesAsync();

            int result = await _analyzer.NumberOfGamesFromMonthAsync(2019, 4);

            Assert.AreEqual(5, result);
        }

        [Test]
        public async Task NumberOfGamesFromMonthAsync_WeHaveNotDataAboutThatTime_WeGet0()
        {
            int result = await _analyzer.NumberOfGamesFromMonthAsync(2019, 4);

            Assert.AreEqual(0, result);
        }

        [Test]
        public async Task NumberOfGamesFromOneYearAsync_Was120Games_WeGet120()
        {
            for(int i = 1; i <= 12; i++)
            {
                DateTime date = new DateTime(2019, i, i);
                var statistics = new ServerActivityStatistics(date)
                {
                    NumberOfGames = 10
                };
                _db.ServerActivityStatistics.Add(statistics);
            }
           
            await _db.SaveChangesAsync();

            int result = await _analyzer.NumberOfGamesFromOneYearAsync(2019);

            Assert.AreEqual(120, result);
        }

        [Test]
        public async Task NumberOfGamesFromOneYearAsync_WeHaveNotDataAboutThatTime_WeGet0()
        {
            int result = await _analyzer.NumberOfGamesFromOneYearAsync(2019);

            Assert.AreEqual(0, result);
        }

        [TestCase(5)]
        [TestCase(10)]
        [TestCase(9)]
        [TestCase(1)]
        [TestCase(4)]
        public async Task NumberOfGamesFromLastDaysAsync_Was10Games_WeGet10(int numberOfDays)
        {

            DateTime date = DateTime.Now;
            var statistics = new ServerActivityStatistics(date)
            {
                NumberOfGames = 2
            };
            _db.ServerActivityStatistics.Add(statistics);

            for (int i = 1; i <= numberOfDays - 1; i++)
            {
                date = DateTime.Now;
                date.AddDays(-i);
                statistics = new ServerActivityStatistics(date)
                {
                    NumberOfGames = 2
                };
                _db.ServerActivityStatistics.Add(statistics);
            }
            await _db.SaveChangesAsync();

            int result = await _analyzer.NumberOfGamesFromLastDaysAsync(numberOfDays);

            Assert.AreEqual(numberOfDays * 2, result);
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }
    }
}

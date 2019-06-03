using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs.AutomapperProfiles.Converters;

namespace YGOProAnalyticsServerTests.Dtos.Automapper.Converters
{
    [TestFixture]
    class DecklistStatisticsToTotalNumberOfWinsConverterTests
    {
        [Test]
        public void Convert_WeHaveDecklistStatistics_WeGetNumberOfDecklistStatistics()
        {
            var randomNumber = new Random().Next(1, 10);
            var converter = new DecklistStatisticsToTotalNumberOfWinsConverter();
            var statistics = new List<DecklistStatistics>();
            int expectedSum = 0;
            for (int i = 0; i < randomNumber; i++)
            {
                var stats = new DecklistStatistics();
                stats.IncrementNumberOfTimesWhenDeckWonByAmount(randomNumber);
                expectedSum += randomNumber;

                statistics.Add(stats);
            }

            int result = converter.Convert(statistics, null);

            Assert.AreEqual(expectedSum, result);
        }
    }
}

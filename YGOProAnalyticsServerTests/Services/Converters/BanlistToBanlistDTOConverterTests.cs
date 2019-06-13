using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Converters;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using System.Linq;
using Moq;
using YGOProAnalyticsServer.Services.Factories.Interfaces;

namespace YGOProAnalyticsServerTests.Services.Converters
{
    [TestFixture]
    class BanlistToBanlistDTOConverterTests
    {
        IBanlistToBanlistDTOConverter _converter;
        Banlist _banlist;
        Mock<IDecksDtosFactory> _decksDtosFactory;

        [SetUp]
        public void SetUp()
        {
            _decksDtosFactory = new Mock<IDecksDtosFactory>();
            _converter = new BanlistToBanlistDTOConverter(_decksDtosFactory.Object);
            _banlist = new Banlist("2019.11 TCG", 1);
            var statistics1 = BanlistStatistics.Create(new DateTime(2020, 4, 29), _banlist);
            statistics1.IncrementHowManyTimesWasUsed();
            var statistics2 = BanlistStatistics.Create(new DateTime(2020, 4, 30), _banlist);
            statistics2.IncrementHowManyTimesWasUsed();
            statistics2.IncrementHowManyTimesWasUsed();
            _banlist.Statistics.Add(statistics1);
            _banlist.Statistics.Add(statistics2);
        }

        [Test]
        public void Convert_ThereIsNoDateLimits_WeGetBanlistsWhichSumStatisticsFromAllDates()
        {
            var dtos = _converter.Convert(new List<Banlist>() { _banlist });

            Assert.AreEqual(3, dtos.First().HowManyTimeWasUsed);
        }

        [Test]
        public void Convert_ThereIsNoDateLimits_WeGetDtoWithValidDTO()
        {
            var dtos = _converter.Convert(new List<Banlist>() { _banlist });

            Assert.AreEqual("2019.11 TCG", dtos.First().Name);
            Assert.AreEqual(2019, dtos.First().ReleaseDate.Year);
            Assert.AreEqual(11, dtos.First().ReleaseDate.Month);
            Assert.AreEqual("TCG", dtos.First().Format);
        }

        [Test]
        public void Convert_ThereIsDateFromLimit_WeGetValidNumberOfTimesWhenBanlistWasUsed()
        {
            var dtos = _converter.Convert(
                banlistToConvert: new List<Banlist>() { _banlist },
                statisticsFrom: new DateTime(2020, 4, 30));

            Assert.AreEqual(2, dtos.First().HowManyTimeWasUsed);
        }

        [Test]
        public void Convert_ThereIsDateToLimit_WeGetValidNumberOfTimesWhenBanlistWasUsed()
        {
            var statistics = BanlistStatistics.Create(new DateTime(2020, 5, 1), _banlist);
            statistics.IncrementHowManyTimesWasUsed();
            _banlist.Statistics.Add(statistics);

            var dtos = _converter.Convert(
                banlistToConvert: new List<Banlist>() { _banlist },
                statisticsFrom: new DateTime(2020, 4, 30),
                statisticsTo: new DateTime(2020, 4, 30));

            Assert.AreEqual(2, dtos.First().HowManyTimeWasUsed);
        }
    }
}

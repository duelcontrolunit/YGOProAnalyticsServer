using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Converters;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Services.Factories.Interfaces;

namespace YGOProAnalyticsServerTests.Services.Converters
{
    [TestFixture]
    class ArchetypeToDtoConverterTests
    {
        IArchetypeToDtoConverter _converter;
        Mock<IDecksDtosFactory> _deckDtosFactory;

        [SetUp]
        public void SetUp()
        {
            _deckDtosFactory = new Mock<IDecksDtosFactory>();
            _converter = new ArchetypeToDtoConverter(_deckDtosFactory.Object);
        }

        [Test]
        public void Convert_WeGetFirstDtoFromCollecti_StatisticsSuccesfullyConverted()
        {
            var statistics = new List<ArchetypeStatistics>();
            var archetypeStatistics = new ArchetypeStatistics(
                new Archetype(Archetype.Default, true),
                new DateTime(2019, 4, 29));
            archetypeStatistics.IncrementNumberOfDecksWhereWasUsedByAmount(10);
            archetypeStatistics.IncrementNumberOfTimesWhenArchetypeWonByAmount(2);
            statistics.Add(archetypeStatistics);
            var statisticsDto = _converter.Convert(statistics).First();

            Assert.Multiple(()=> {
                Assert.AreEqual(
                    archetypeStatistics.NumberOfDecksWhereWasUsed,
                    statisticsDto.NumberOfDecksWhereWasUsed);
                Assert.AreEqual(
                    archetypeStatistics.NumberOfTimesWhenArchetypeWon,
                    statisticsDto.NumberOfTimesWhenArchetypeWon);
                Assert.AreEqual(
                   archetypeStatistics.DateWhenArchetypeWasUsed,
                   statisticsDto.DateWhenArchetypeWasUsed);
            });
        }
    }
}

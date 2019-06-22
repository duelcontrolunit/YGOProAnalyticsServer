using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _converter = new ArchetypeToDtoConverter(_deckDtosFactory.Object);
        }

        [Test]
        public async Task Convert_WePassEmptyArchetypesCollectionAsArgument_WeGetEmptyArchetypeWithHowManyWinsAndHowManyWasUsedCollection()
        {
            CollectionAssert.IsEmpty(_converter.Convert(new List<Archetype>()));
        }

        [Test]
        public async Task Convert_WePassArchetypesCollectionWithOneElement_FirstElementConvertedCollectionHaveValidTotalNumberOfGames()
        {
            List<Archetype> archetypes = _getArchetypesCollectionForTest();

            var dto = _converter.Convert(archetypes).First();

            Assert.AreEqual(215, dto.HowManyWasUsed);
        }

        [Test]
        public async Task Convert_WePassArchetypesCollectionWithOneElement_FirstElementConvertedCollectionHaveValidTotalNumberOfWins()
        {
            List<Archetype> archetypes = _getArchetypesCollectionForTest();

            var dto = _converter.Convert(archetypes).First();

            Assert.AreEqual(51, dto.HowManyWon);
        }

        [Test]
        public async Task Convert_WePassDateForArgument_FirstElementConvertedCollectionHaveValidTotalNumberOfGames()
        {
            List<Archetype> archetypes = _getArchetypesCollectionForTest();

            var dto = _converter.Convert(archetypes, statisticsFrom: new DateTime(2019, 4, 30)).First();

            Assert.AreEqual(205, dto.HowManyWasUsed);
        }

        [Test]
        public async Task Convert_WePassDateForArgument_FirstElementConvertedCollectionHaveValidTotalNumberOfWins()
        {
            List<Archetype> archetypes = _getArchetypesCollectionForTest();

            var dto = _converter.Convert(archetypes, statisticsFrom: new DateTime(2019, 4, 30)).First();

            Assert.AreEqual(50, dto.HowManyWon);
        }

        [Test]
        public async Task Convert_WeWantGetDataToSecondDay_FirstElementConvertedCollectionHaveValidTotalNumberOfGames()
        {
            List<Archetype> archetypes = _getArchetypesCollectionForTest();

            var dto = _converter.Convert(archetypes, statisticsTo: new DateTime(2019, 4, 30)).First();

            Assert.AreEqual(15, dto.HowManyWasUsed);
        }

        [Test]
        public async Task Convert_WeWantGetDataToSecondDay_FirstElementConvertedCollectionHaveValidTotalNumberOfWins()
        {
            List<Archetype> archetypes = _getArchetypesCollectionForTest();

            var dto = _converter.Convert(archetypes, statisticsTo: new DateTime(2019, 4, 30)).First();

            Assert.AreEqual(1, dto.HowManyWon);
        }

        [Test]
        public async Task Convert_WeWantGetDataOnlyFromSecondDay_FirstElementConvertedCollectionHaveValidTotalNumberOfGames()
        {
            List<Archetype> archetypes = _getArchetypesCollectionForTest();

            var dto = _converter.Convert(archetypes, new DateTime(2019, 4, 30), new DateTime(2019, 4, 30)).First();

            Assert.AreEqual(5, dto.HowManyWasUsed);
        }

        [Test]
        public async Task Convert_WeWantGetDataOnlyFromSecondDay_FirstElementConvertedCollectionHaveValidTotalNumberOfWins()
        {
            List<Archetype> archetypes = _getArchetypesCollectionForTest();

            var dto = _converter.Convert(archetypes, new DateTime(2019, 4, 30), new DateTime(2019, 4, 30)).First();

            Assert.AreEqual(0, dto.HowManyWon);
        }

        private static List<Archetype> _getArchetypesCollectionForTest()
        {
            var archetypes = new List<Archetype>();
            var archetype = new Archetype("TestArchetype", false);

            var statistics1 = new ArchetypeStatistics(archetype, new DateTime(2019, 4, 29));
            statistics1.IncrementNumberOfDecksWhereWasUsedByAmount(10);
            statistics1.IncrementNumberOfTimesWhenArchetypeWonByAmount(1);

            var statistics2 = new ArchetypeStatistics(archetype, new DateTime(2019, 4, 30));
            statistics2.IncrementNumberOfDecksWhereWasUsedByAmount(5);
            statistics2.IncrementNumberOfTimesWhenArchetypeWonByAmount(0);

            var statistics3 = new ArchetypeStatistics(archetype, new DateTime(2019, 5, 2));
            statistics3.IncrementNumberOfDecksWhereWasUsedByAmount(200);
            statistics3.IncrementNumberOfTimesWhenArchetypeWonByAmount(50);

            archetype.Statistics.Add(statistics1);
            archetype.Statistics.Add(statistics2);
            archetype.Statistics.Add(statistics3);
            archetypes.Add(archetype);

            return archetypes;
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

            Assert.Multiple(() =>
            {
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

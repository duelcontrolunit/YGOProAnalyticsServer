using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Converters;
using YGOProAnalyticsServer.Services.Factories.Interfaces;

namespace YGOProAnalyticsServerTests.Services.Converters
{
    [TestFixture]
    class DecklistToDecklistDTOConverterTests
    {
        private const string _validDecklistName = "ValidDecklist";
        private const string _validArchatypeName = "validArchetype";
        private const int _validDecklistId = 14;
        private readonly DateTime _date = new DateTime(1997, 04, 29);

        DecklistToDecklistDtoConverter _converter;
        Mock<IDecksDtosFactory> _decksDtosFactoryMock;


        [SetUp]
        public void SetUp()
        {
            _decksDtosFactoryMock = new Mock<IDecksDtosFactory>();
        }

        /// <seealso cref="IDecksDtosFactory"/>
        [Test]
        public void Convert_WeGetValidDTO()
        {
            _decksDtosFactoryMock
                .Setup(x => x.CreateDeckDto(It.IsAny<Decklist>()))
                .Returns(new DeckDTO());
            _decksDtosFactoryMock
               .Setup(x => x.CreateExtraDeckDto(It.IsAny<Decklist>()))
               .Returns(new ExtraDeckDTO());
            _decksDtosFactoryMock
               .Setup(x => x.CreateMainDeckDto(It.IsAny<Decklist>()))
               .Returns(new MainDeckDTO());
            _initConverter();

            var dto = _converter.Convert(_getValidDecklist());

            Assert.Multiple(() =>
            {
                Assert.AreEqual(_validDecklistId, dto.DecklistId);
                Assert.AreEqual(_validArchatypeName, dto.Archetype);
                Assert.AreEqual(_validDecklistName, dto.Name);
                Assert.AreEqual(_date, dto.WhenDeckWasFirstPlayed);
                Assert.AreEqual(3, dto.Statistics.FirstOrDefault()?.NumberOfTimesWhenDeckWasUsed);
                Assert.AreEqual(1, dto.Statistics.FirstOrDefault()?.NumberOfTimesWhenDeckWon);
                Assert.AreEqual(_date.AddDays(2), dto.Statistics.FirstOrDefault()?.DateWhenDeckWasUsed);

                //For this test will be good if that three properties will be not null.
                //I don`t want test deck dto factory twice.
                Assert.NotNull(dto.MainDeck);
                Assert.NotNull(dto.ExtraDeck);
                Assert.NotNull(dto.SideDeck);
            });

        }

        private void _initConverter()
        {
            _converter = new DecklistToDecklistDtoConverter(_decksDtosFactoryMock.Object);
        }

        private Decklist _getValidDecklist()
        {
            var decklist = new Decklist(new List<Card>(), new List<Card>(), new List<Card>())
            {
                Name = _validDecklistName,
                Archetype = new Archetype(_validArchatypeName, false),
                WhenDecklistWasFirstPlayed = _date
            };

            decklist
                .GetType()
                .GetProperty(nameof(Decklist.Id))
                .SetValue(decklist, _validDecklistId);
            decklist.DecklistStatistics.Add(
                _getValidStatisticsForValidDeck()
            );

            return decklist;
        }

        private DecklistStatistics _getValidStatisticsForValidDeck()
        {
            DecklistStatistics statistics = new DecklistStatistics();
            statistics.IncrementNumberOfTimesWhenDeckWasUsed();
            statistics.IncrementNumberOfTimesWhenDeckWasUsed();
            statistics.IncrementNumberOfTimesWhenDeckWasUsed();
            statistics.IncrementNumberOfTimesWhenDeckWon();

            statistics.GetType()
                .GetProperty(nameof(DecklistStatistics.DateWhenDeckWasUsed))
                .SetValue(statistics, _date.AddDays(2));

            statistics.GetType()
               .GetProperty(nameof(DecklistStatistics.Id))
               .SetValue(statistics, 44);

            return statistics;
        }
    }
}

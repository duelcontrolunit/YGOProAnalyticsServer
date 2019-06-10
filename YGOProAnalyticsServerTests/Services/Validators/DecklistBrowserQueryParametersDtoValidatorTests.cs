using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Validators;
using YGOProAnalyticsServer.Services.Validators.Interfaces;

namespace YGOProAnalyticsServerTests.Services.Validators
{
    [TestFixture]
    class DecklistBrowserQueryParametersDtoValidatorTests
    {
        IDecklistBrowserQueryParametersDtoValidator _validator;
        Mock<IDateValidator> _dateValidatorMock;

        [SetUp]
        public void SetUp()
        {
            _dateValidatorMock = new Mock<IDateValidator>();
            _dateValidatorMock
                .Setup(x => x.IsValidFormat(
                    It.IsRegex(@"^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$"),
                    DateFormat.yyyy_MM_dd))
                .Returns(true);
            _validator = new DecklistBrowserQueryParametersDtoValidator(_dateValidatorMock.Object);
        }

        [TestCase("", -5, 10, 1, null, null)]
        [TestCase("", 0, 10, 1, null, null)]
        [TestCase("", 5, 10, 1, "29/05/2019", null)]
        [TestCase("", 5, 0, 1, null, null)]
        [TestCase("", 5, -1, 1, null, null)]
        [TestCase("", 5, 10, 1, null, "29/05/2019")]
        [TestCase("", 5, 10, 1, "29/05/2019", null)]
        [TestCase("", 5, 10, 1, "29/05/2019", "29/05/2019")]
        [TestCase("", 5, 1, 0, null, null)]
        [TestCase("", 5, 1, -1, "29/05/2019", "29/05/2019")]
        [TestCase("", 5, 1, -10, null, null)]
        [TestCase("", 5, 1, -1, "2019-12-12", "2019-12-12", -5)]
        [TestCase("", 5, 1, -1, "2019-12-12", "2019-12-12", 0)]
        public void IsValid_InvalidParamOrParams_ReturnsFalse(
            string archetypeName,
            int banlistId,
            int minNumberOfGames,
            int pageNumber,
            string statisticsFromDate,
            string statisticsToDate,
            int numberOfResults = -1)
        {
            var dto = new DecklistBrowserQueryParametersDTO() {
                ArchetypeName = archetypeName,
                BanlistId = banlistId,
                MinNumberOfGames = minNumberOfGames,
                PageNumber = pageNumber,
                StatisticsFromDate = statisticsFromDate,
                StatisticsToDate = statisticsToDate
            };

            Assert.IsFalse(_validator.IsValid(dto));
        }

        [TestCase("", 1, 10, 1, "", "")]       
        [TestCase("", 100, 2, 1, "", "")]
        [TestCase("", 1, 10, 1, "2019-12-12", "")]
        [TestCase("", 100, 2, 1, "", "2019-12-12")]
        [TestCase("", 100, 2, 1, "2019-12-11", "2019-12-12")]
        [TestCase("", 100, 2, 1, "2019-12-11", "2019-12-12", -1)]
        [TestCase("", 100, 2, 1, "2019-12-11", "2019-12-12", 100)]
        [TestCase("", 100, 2, 1, "2019-12-11", "2019-12-12", 1)]
        public void IsValid_ValidParams_ReturnsTrue(
          string archetypeName,
          int banlistId,
          int minNumberOfGames,
          int pageNumber,
          string statisticsFromDate,
          string statisticsToDate,
          int numberOfResults = -1)
        {
            var dto = new DecklistBrowserQueryParametersDTO()
            {
                ArchetypeName = archetypeName,
                BanlistId = banlistId,
                MinNumberOfGames = minNumberOfGames,
                PageNumber = pageNumber,
                StatisticsFromDate = statisticsFromDate,
                StatisticsToDate = statisticsToDate
            };

            Assert.IsTrue(_validator.IsValid(dto));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        [TestCase(42342)]
        [TestCase(100000)]
        public void IsValidPageNumber_IsValid_ReturnsTrue(int pageNumber)
        {
            Assert.IsTrue(_validator.IsValidPageNumber(new DecklistBrowserQueryParametersDTO() { PageNumber = pageNumber }));
        }

        [TestCase(-2)]
        [TestCase(0)]
        [TestCase(-100)]
        public void IsValidPageNumber_IsInvalid_ReturnsTrue(int pageNumber)
        {
            Assert.IsFalse(_validator.IsValidPageNumber(new DecklistBrowserQueryParametersDTO() { PageNumber = pageNumber }));
        }

        [TestCase(-1)]
        [TestCase(1)]
        [TestCase(4)]
        public void IsValidBanlistId_IsValid_ReturnsTrue(int banlistId)
        {
            Assert.IsTrue(_validator.IsValidBanlistId(new DecklistBrowserQueryParametersDTO() { BanlistId = banlistId }));
        }

        [TestCase("Abc")]
        [TestCase(null)]
        [TestCase("")]
        public void IsValidArchetypeName_IsValid_ReturnsTrue(string archetypeName)
        {
            Assert.IsTrue(_validator.IsValidArchetypeName(
                new DecklistBrowserQueryParametersDTO() { ArchetypeName = archetypeName })
             );
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(-100)]
        public void IsValidMinNumberOfGames_IsInvalid_ReturnsFalse(int numberOfGames)
        {
            Assert.IsFalse(_validator.IsValidMinNumberOfGames(
               new DecklistBrowserQueryParametersDTO() { MinNumberOfGames = numberOfGames })
            );
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void IsValidMinNumberOfGames_IsValid_ReturnsTrue(int numberOfGames)
        {
            Assert.IsTrue(_validator.IsValidMinNumberOfGames(
               new DecklistBrowserQueryParametersDTO() { MinNumberOfGames = numberOfGames })
            );
        }

        [TestCase("")]
        [TestCase("2019-06-03")]
        public void IsValidStatisticsFromDate_IsValid_ReturnsTrue(string dateAsString)
        {
            Assert.IsTrue(_validator.IsValidStatisticsFromDate(
              new DecklistBrowserQueryParametersDTO() { StatisticsFromDate = dateAsString })
           );
        }

        [TestCase(null)]
        [TestCase("2019/06/03")]
        [TestCase("2019.06.03")]
        public void IsValidStatisticsFromDate_IsInvalid_ReturnsFalse(string dateAsString)
        {
            Assert.IsFalse(_validator.IsValidStatisticsFromDate(
              new DecklistBrowserQueryParametersDTO() { StatisticsFromDate = dateAsString })
           );
        }

        [TestCase("")]
        [TestCase("2019-06-03")]
        public void IsValidStatisticsToDate_IsValid_ReturnsTrue(string dateAsString)
        {
            Assert.IsTrue(_validator.IsValidStatisticsToDate(
              new DecklistBrowserQueryParametersDTO() { StatisticsToDate = dateAsString })
           );
        }

        [TestCase(null)]
        [TestCase("2019/06/03")]
        [TestCase("2019.06.03")]
        public void IsValidStatisticsToDate_IsInvalid_ReturnsFalse(string dateAsString)
        {
            Assert.IsFalse(_validator.IsValidStatisticsToDate(
              new DecklistBrowserQueryParametersDTO() { StatisticsToDate = dateAsString })
           );
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(150)]
        [TestCase(-1)]
        public void IsValidNumberOfResults_IsValid_ReturnsTrue(int numberOfResults)
        {
            Assert.IsTrue(_validator.IsValidNumberOfResults(new DecklistBrowserQueryParametersDTO() {
                NumberOfResults = numberOfResults
            }));
        }

        [TestCase(0)]
        [TestCase(-100)]
        [TestCase(-150)]
        public void IsValidNumberOfResults_IsInValid_ReturnsFalse(int numberOfResults)
        {
            Assert.IsFalse(_validator.IsValidNumberOfResults(new DecklistBrowserQueryParametersDTO()
            {
                NumberOfResults = numberOfResults
            }));
        }
    }
}

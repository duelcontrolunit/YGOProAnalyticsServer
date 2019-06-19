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
    class ArchetypeBrowserQueryParamsValidatorTests
    {
        IArchetypeBrowserQueryParamsValidator _validator;
        Mock<IDateValidator> _dateValidatorMock;

        [SetUp]
        public void SetUp()
        {
            _dateValidatorMock = new Mock<IDateValidator>();
            // because I dont want test IDateValidator twice
            _dateValidatorMock
                .Setup(x => x.IsValidFormat(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            _validator = new ArchetypeBrowserQueryParamsValidator(_dateValidatorMock.Object);
        }

        [TestCase(1, "Neutral", 1, 1, "", "")]
        [TestCase(1, "Neutral", 1, 1, "2019-04-29", "2019-6-3")]
        [TestCase(100, "Neutral", 1, 1, "2019-04-29", "2019-6-3")]
        [TestCase(1, "Neutral", 100, 1, "2019-04-29", "2019-6-3")]
        [TestCase(1, "Neutral", 100, -1, "2019-04-29", "2019-6-3")]
        [TestCase(1, "Neutral", 100, -1, "2019-04-29", "")]
        [TestCase(1, "Neutral", 100, -1, "", "2019-6-3")]
        public void IsValid_IsValid_ReturnsTrue(
                int pageNumber,
                string archetypeName,
                int minNumberOfGames,
                int numberOfResults,
                string statisticsFromDate,
                string statisticsToDate
            )
        {
            var dto = new ArchetypeBrowserQueryParams() {
                PageNumber = pageNumber,
                ArchetypeName = archetypeName,
                MinNumberOfGames = minNumberOfGames,
                NumberOfResults = numberOfResults,
                StatisticsFromDate = statisticsFromDate,
                StatisticsToDate = statisticsToDate
            };

            Assert.IsTrue(_validator.IsValid(dto));
        }

        [TestCase(1, "Neutral", 1, -2, "", "")]
        [TestCase(1, "Neutral", 1, 0, "", "")]
        [TestCase(0, "Neutral", 1, 1, "", "")]
        [TestCase(-1, "Neutral", 1, 1, "", "")]
        public void IsInvalid_IsInvalid_ReturnsFalse(
               int pageNumber,
               string archetypeName,
               int minNumberOfGames,
               int numberOfResults,
               string statisticsFromDate,
               string statisticsToDate
           )
        {
            var dto = new ArchetypeBrowserQueryParams()
            {
                PageNumber = pageNumber,
                ArchetypeName = archetypeName,
                MinNumberOfGames = minNumberOfGames,
                NumberOfResults = numberOfResults,
                StatisticsFromDate = statisticsFromDate,
                StatisticsToDate = statisticsToDate
            };

            Assert.IsFalse(_validator.IsValid(dto));
        }
    }
}

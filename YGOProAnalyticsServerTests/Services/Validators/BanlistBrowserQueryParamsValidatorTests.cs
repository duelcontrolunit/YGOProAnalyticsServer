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
    class BanlistBrowserQueryParamsValidatorTests
    {
        IBanlistBrowserQueryParamsValidator _validator;
        Mock<IDateValidator> _dateValidatorMock;

        [SetUp]
        public void SetUp()
        {
            _dateValidatorMock = new Mock<IDateValidator>();
            _validator = new BanlistBrowserQueryParamsValidator(_dateValidatorMock.Object);
        }

        [TestCase(1, 1, 10, "", "")]
        [TestCase(1, 1, 10, "2019-12-11", "2019-12-12")]
        [TestCase(1, 1, -1, "2019-12-11", "2019-12-12")]
        [TestCase(1, 1, -1, "", "2019-12-12")]
        [TestCase(1, 1, -1, "2019-12-11", "")]
        [TestCase(100, 100, 100, "", "")]
        public void IsValid_ParamsValidValid_ReturnsTrue(int pageNumber, int minNumberOfGames, int numberOfResults,
            string statisticsFromDate, string statisticsToDate)
        {
            _dateValidatorMock
                .Setup(x => x.IsValidFormat(It.IsAny<string>(), DateFormat.yyyy_MM_dd))
                .Returns(true);
            var dto = new BanlistBrowserQueryParams() {
                PageNumber = pageNumber,
                MinNumberOfGames = minNumberOfGames,
                NumberOfResults = numberOfResults,
                StatisticsFromDate = statisticsFromDate,
                StatisticsToDate = statisticsToDate
            };

            Assert.IsTrue(_validator.IsValid(dto));
        }

        [TestCase(-1, 1, 10, "", "")]
        [TestCase(0, 0, -2, null, null)]
        [TestCase(0, 1, 10, "", "")]
        [TestCase(1, 0, 10, "", "")]
        [TestCase(1, 1, -2, "", "")]
        [TestCase(1, 1, 10, null, "")]
        [TestCase(1, 1, 10, "", null)]
        public void IsValid_InvaldParams_ReturnsFalse(int pageNumber, int minNumberOfGames, int numberOfResults,
            string statisticsFromDate, string statisticsToDate)
        {
            _dateValidatorMock
               .Setup(x => x.IsValidFormat(It.IsAny<string>(), DateFormat.yyyy_MM_dd))
               .Returns(false);
            var dto = new BanlistBrowserQueryParams()
            {
                PageNumber = pageNumber,
                MinNumberOfGames = minNumberOfGames,
                NumberOfResults = numberOfResults,
                StatisticsFromDate = statisticsFromDate,
                StatisticsToDate = statisticsToDate
            };
            Assert.IsFalse(_validator.IsValid(dto));
        }
    }
}

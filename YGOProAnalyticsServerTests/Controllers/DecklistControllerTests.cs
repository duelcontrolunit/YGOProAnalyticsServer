using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer;
using YGOProAnalyticsServer.Controllers;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Services.Others.Interfaces;
using YGOProAnalyticsServer.Services.Validators.Interfaces;
using YGOProAnalyticsServerTests.TestingHelpers;

namespace YGOProAnalyticsServerTests.Controllers
{
    [TestFixture]
    class DecklistControllerTests
    {
        DecklistController _decklistController;
        YgoProAnalyticsDatabase _db;
        Mock<IDecklistToDecklistDtoConverter> _decklistToDtoConverter;
        Mock<IDecklistService> _decklistService;
        Mock<IAdminConfig> _adminConfigMock;
        Mock<IMapper> _mapperMock;
        Mock<IDecklistBrowserQueryParametersDtoValidator> _decklistBrowserQueryParamsValidator;

        [SetUp]
        public void SetUp()
        {
            _db = new YgoProAnalyticsDatabase(SqlInMemoryHelper.SqlLiteOptions<YgoProAnalyticsDatabase>());
            _db.Database.EnsureCreated();
            _decklistToDtoConverter = new Mock<IDecklistToDecklistDtoConverter>();
            _decklistService = new Mock<IDecklistService>();
            _adminConfigMock = new Mock<IAdminConfig>();
            _mapperMock = new Mock<IMapper>();
            _decklistBrowserQueryParamsValidator = new Mock<IDecklistBrowserQueryParametersDtoValidator>();
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        [Test]
        public async Task Get_DecklistNotExist_WeGet404Response()
        {
            _decklistService
                .Setup(x => x.GetByIdWithAllDataIncluded(15))
                .ReturnsAsync((Decklist)null);
            _initController();

            var actionResult = await _decklistController.Get(15);

            Assert.IsTrue(actionResult is NotFoundObjectResult || actionResult is NotFoundResult);
        }

        [Test]
        public async Task Get_DecklistExist_WeGetThatDecklistAsJson()
        {
            var validDecklist = _getValidDecklist();
            _decklistService
                .Setup(x => x.GetByIdWithAllDataIncluded(15))
                .ReturnsAsync(validDecklist);
            _decklistToDtoConverter
                .Setup(x => x.Convert(validDecklist))
                .Returns(_getValidDto());
            _initController();

            var actionResult = await _decklistController.Get(15);

            Assert.IsTrue(actionResult is JsonResult);
        }

        [TestCase(1, -5)]
        public async Task FindAll_InvalidParam_WeGetBadRequestResponse(
            int pageNumber,
            int banlistId,
            string archetypeName,
            int minNumberOfGames,
            string statisticsFromDate,
            string statisticsToDate)
        {
            var queryParamsDto = new DecklistBrowserQueryParametersDTO() {
                PageNumber = pageNumber,
                BanlistId = banlistId,
                ArchetypeName = archetypeName,
                MinNumberOfGames = minNumberOfGames,
                StatisticsFromDate = statisticsFromDate,
                StatisticsToDate = statisticsToDate
            };


        }

        private void _initController()
        {
            _decklistController = new DecklistController(
                _db,
                _decklistToDtoConverter.Object,
                _decklistService.Object,
                _adminConfigMock.Object,
                _mapperMock.Object,
                _decklistBrowserQueryParamsValidator.Object);
        }

        private Decklist _getValidDecklist()
        {
            return new Decklist(
                new List<Card>(),
                new List<Card>(),
                new List<Card>()){
                Name = "ValidName",
                Archetype = new Archetype("validArchetype", false),
                WhenDecklistWasFirstPlayed = new DateTime(1997, 04, 29)
            };
        }

        /// <summary>
        /// For <see cref="_getValidDecklist"/>
        /// </summary>
        private DecklistWithStatisticsDTO _getValidDto()
        {
            return new DecklistWithStatisticsDTO() {
                Archetype = "validArchetype",
                DecklistId = 14,
                Name = "ValidDecklist",
                WhenDeckWasFirstPlayed = new DateTime(1997, 04, 29)
            };
        }
    }
}

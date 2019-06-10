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
        Mock<INumberOfResultsHelper> _numberOfResultsHelper;

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
            _numberOfResultsHelper = new Mock<INumberOfResultsHelper>();
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

        [TestCase(1, -5, "Blue-eyes", 10, "2018-12-12", "2018-12-12")]
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

            _decklistBrowserQueryParamsValidator
                .Setup(x => x.IsValid(queryParamsDto))
                .Returns(false);            
            _initController();

            var response = await _decklistController.FindAll(queryParamsDto);

            Assert.IsTrue(response is BadRequestObjectResult
                          || response is BadRequestResult);
        }
     
        [TestCase(1, -1, "Blue-eyes", 10, "2018-12-12", "2018-12-12")]
        public async Task FindAll_AllParamsValid_WeGetJsonResponse(
            int pageNumber,
            int banlistId,
            string archetypeName,
            int minNumberOfGames,
            string statisticsFromDate,
            string statisticsToDate)
        {
            var queryParamsDto = new DecklistBrowserQueryParametersDTO()
            {
                PageNumber = pageNumber,
                BanlistId = banlistId,
                ArchetypeName = archetypeName,
                MinNumberOfGames = minNumberOfGames,
                StatisticsFromDate = statisticsFromDate,
                StatisticsToDate = statisticsToDate
            };

            _decklistBrowserQueryParamsValidator
                .Setup(x => x.IsValid(queryParamsDto))
                .Returns(true);

            var dateTimeFrom = DateTime.Parse(statisticsFromDate);
            var dateTimeTo = DateTime.Parse(statisticsToDate);
            var emptyListOfDecklists = new List<Decklist>();
            _decklistService
                .Setup(x => x.FindAll(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    minNumberOfGames,
                    banlistId,
                    archetypeName,
                    dateTimeFrom,
                    dateTimeTo,
                    true
                ))
                .ReturnsAsync(emptyListOfDecklists);
            _decklistToDtoConverter
                .Setup(x => x.Convert(emptyListOfDecklists, dateTimeFrom, dateTimeTo))
                .Returns(new List<DecklistWithNumberOfGamesAndWinsDTO>());
            _adminConfigMock.Setup(x => x.DefaultNumberOfResultsPerBrowserPage).Returns(100);
            _initController();
            var result = await _decklistController.FindAll(queryParamsDto);

            Assert.IsTrue(result is JsonResult);
        }

        private void _initController()
        {
            _decklistController = new DecklistController(
                _db,
                _decklistToDtoConverter.Object,
                _decklistService.Object,
                _adminConfigMock.Object,
                _mapperMock.Object,
                _decklistBrowserQueryParamsValidator.Object,
                _numberOfResultsHelper.Object);
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

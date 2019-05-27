﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Controllers;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Services.Others.Interfaces;
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

        [SetUp]
        public void SetUp()
        {
            _db = new YgoProAnalyticsDatabase(SqlInMemoryHelper.SqlLiteOptions<YgoProAnalyticsDatabase>());
            _db.Database.EnsureCreated();
            _decklistToDtoConverter = new Mock<IDecklistToDecklistDtoConverter>();
            _decklistService = new Mock<IDecklistService>();
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

        private void _initController()
        {
            _decklistController = new DecklistController(_db, _decklistToDtoConverter.Object, _decklistService.Object);
        }

        private Decklist _getValidDecklist()
        {
            return new Decklist(
                "ValidDecklist",
                new Archetype("validArchetype", false),
                new DateTime(1997, 04, 29));
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

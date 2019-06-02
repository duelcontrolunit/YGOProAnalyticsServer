using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecklistController : ControllerBase
    {
        readonly YgoProAnalyticsDatabase _db;
        readonly IDecklistToDecklistDtoConverter _decklistToDtoConverter;
        readonly IDecklistService _decklistService;
        readonly IAdminConfig _config;
        readonly IMapper _mapper;

        public DecklistController(
            YgoProAnalyticsDatabase db,
            IDecklistToDecklistDtoConverter decklistToDtoConverter,
            IDecklistService decklistService,
            IAdminConfig config,
            IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _decklistToDtoConverter = decklistToDtoConverter ?? throw new ArgumentNullException(nameof(decklistToDtoConverter));
            _decklistService = decklistService ?? throw new ArgumentNullException(nameof(decklistService));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> FindAll([FromQuery] DecklistBrowserQueryParametersDTO queryParams)
        {
            //TODO : Validation
            DateTime? statisticsFrom = null;
            DateTime? statisticsTo = null;
            if (!string.IsNullOrEmpty(queryParams.StatisticsFromDate))
            {
                statisticsFrom = DateTime.Parse(queryParams.StatisticsFromDate);
            }

            if (!string.IsNullOrEmpty(queryParams.StatisticsToDate))
            {
                statisticsTo = DateTime.Parse(queryParams.StatisticsToDate);
            }

            var decklists = await _decklistService.FindAll(
                howManyTake: _config.DefaultNumberOfResultsPerBrowserPage,
                howManySkip: _config.DefaultNumberOfResultsPerBrowserPage * (queryParams.PageNumber - 1),
                minNumberOfGames: queryParams.MinNumberOfGames,
                banlistId: queryParams.BanlistId,
                archetypeName: queryParams.ArchetypeName,
                statisticsFrom: statisticsFrom,
                statisticsTo: statisticsTo);

            var decklistDtos = _decklistToDtoConverter.Convert(
                decklists,
                statisticsFrom,
                statisticsTo);

            return new JsonResult(decklistDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var decklist = await _decklistService.GetByIdWithAllDataIncluded(id);           
            if(decklist == null)
            {
                return NotFound("There is no decklist with given id.");
            }

            var decklistDto = _decklistToDtoConverter.Convert(decklist);

            return new JsonResult(decklistDto);
        }
    }
}
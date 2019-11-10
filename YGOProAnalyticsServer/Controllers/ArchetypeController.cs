using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Services.Others.Interfaces;
using YGOProAnalyticsServer.Services.Validators;
using YGOProAnalyticsServer.Services.Validators.Interfaces;

namespace YGOProAnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchetypeController : ControllerBase
    {
        readonly IArchetypeService _archetypeService;
        readonly IArchetypeBrowserQueryParamsValidator _queryParamsValidator;
        readonly INumberOfResultsHelper _numberOfResultsHelper;
        readonly IArchetypeToDtoConverter _archetypeToDtoConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchetypeController"/> class.
        /// </summary>
        /// <param name="archetypeService">The archetype service.</param>
        /// <param name="queryParamsValidator">The query parameters validator.</param>
        /// <param name="numberOfResultsHelper">The number of results helper.</param>
        /// <param name="archetypeToDtoConverter">The archetype to dto converter.</param>
        /// <exception cref="ArgumentNullException">
        /// archetypeService
        /// or
        /// queryParamsValidator
        /// or
        /// numberOfResultsHelper
        /// or
        /// archetypeToDtoConverter
        /// </exception>
        public ArchetypeController(
            IArchetypeService archetypeService,
            IArchetypeBrowserQueryParamsValidator queryParamsValidator,
            INumberOfResultsHelper numberOfResultsHelper,
            IArchetypeToDtoConverter archetypeToDtoConverter)
        {
            _archetypeService = archetypeService ?? throw new ArgumentNullException(nameof(archetypeService));
            _queryParamsValidator = queryParamsValidator ?? throw new ArgumentNullException(nameof(queryParamsValidator));
            _numberOfResultsHelper = numberOfResultsHelper ?? throw new ArgumentNullException(nameof(numberOfResultsHelper));
            _archetypeToDtoConverter = archetypeToDtoConverter ?? throw new ArgumentNullException(nameof(archetypeToDtoConverter));
        }

        /// <summary>
        /// Warning! It returns only pure archetypes.
        /// </summary>
        [HttpGet("ArchetypeListWithIdsAndNames")]
        public async Task<IActionResult> GetArchetypeListWithIdsAndNames()
        {
            return Ok(await _archetypeService.GetPureArchetypeListWithIdsAndNamesAsNoTrackingFromCache());
        }

        [HttpGet]
        public async Task<IActionResult> FindAll([FromQuery]ArchetypeBrowserQueryParams queryParams)
        {
            if (!_queryParamsValidator.IsValid(queryParams))
            {
                return BadRequest("Invalid params. Please report it to administrator.");
            }

            DateTime? statisticsFrom = null;
            DateTime? statisticsTo = null;
            if (!string.IsNullOrEmpty(queryParams.StatisticsFromDate))
            {
                statisticsFrom = DateTime
                    .ParseExact(
                    queryParams.StatisticsFromDate,
                    DateFormat.yyyy_MM_dd,
                    CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(queryParams.StatisticsToDate))
            {
                statisticsTo = DateTime
                    .ParseExact(
                    queryParams.StatisticsToDate,
                    DateFormat.yyyy_MM_dd,
                    CultureInfo.InvariantCulture);
            }

            var archetypes = await _archetypeService.FindAllQuery(
                    minNumberOfGames: queryParams.MinNumberOfGames,
                    archetypeName: queryParams.ArchetypeName,
                    statisticsFromDate: statisticsFrom,
                    statisticsToDate: statisticsTo,
                    includeCards: false,
                    includeDecks: false,
                    OrderByDescendingByNumberOfGames: queryParams.OrderByDescendingByNumberOfGames
                );

            int numberOfResultsPerPage = _numberOfResultsHelper.GetNumberOfResultsPerPage(queryParams.NumberOfResults);
            var numberOfPages = Convert.ToInt32(
                    Math.Ceiling(
                        ((double)(archetypes.Count()) / (double)(numberOfResultsPerPage))
                    )
                );

            var archetypesToActualPage = archetypes
                    .Skip(numberOfResultsPerPage * (queryParams.PageNumber - 1))
                    .Take(numberOfResultsPerPage)
                    .ToList();

            var dtos = _archetypeToDtoConverter.Convert(archetypesToActualPage, statisticsFrom, statisticsTo);

            return Ok(new ArchetypeBrowserResultsDTO(numberOfPages, dtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var archetype = await _archetypeService.GetDataForConcreteArchetypePage(id);
            if (archetype == null)
            {
                return NotFound($"Archetype with id equal {id} not found.");
            }

            var dto = _archetypeToDtoConverter.Convert(archetype);
            dto.Statistics = dto.Statistics.OrderBy(x => x.DateWhenArchetypeWasUsed);
            return Ok(dto);
        }
    }
}
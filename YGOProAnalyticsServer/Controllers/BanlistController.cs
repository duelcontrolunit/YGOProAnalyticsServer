using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class BanlistController : ControllerBase
    {
        readonly IBanlistService _banlistService;
        readonly IBanlistBrowserQueryParamsValidator _validator;
        readonly INumberOfResultsHelper _numberOfResultsHelper;
        readonly IBanlistToBanlistDTOConverter _banlistToDtoConverter;

        public BanlistController(
            IBanlistService banlistService,
            IBanlistBrowserQueryParamsValidator validator,
            INumberOfResultsHelper numberOfResultsHelper,
            IBanlistToBanlistDTOConverter banlistToDtoConverter)
        {
            _banlistService = banlistService ?? throw new ArgumentNullException(nameof(banlistService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _numberOfResultsHelper = numberOfResultsHelper ?? throw new ArgumentNullException(nameof(numberOfResultsHelper));
            _banlistToDtoConverter = banlistToDtoConverter ?? throw new ArgumentNullException(nameof(banlistToDtoConverter));
        }

        [HttpGet("ListOfBanlistsWithIdAndNames")]
        public async Task<IActionResult> GetListOfBanlistsWithIdAndNames()
        {
            return Ok(await _banlistService.GetListOfBanlistsNamesAndIdsAsNoTrackingFromCache());
        }

        [HttpGet]
        public async Task<IActionResult> FindAll([FromQuery] BanlistBrowserQueryParams queryParams)
        {
            if (!_validator.IsValid(queryParams))
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

            var banlists = await _banlistService
                .FindAllQuery(
                    minNumberOfGames: queryParams.MinNumberOfGames,
                    formatOrName: queryParams.FormatOrName,
                    statisticsFromDate: statisticsFrom,
                    statisticsToDate: statisticsTo
                );

            int numberOfResultsPerPage = _numberOfResultsHelper.GetNumberOfResultsPerPage(queryParams.NumberOfResults);
            var numberOfPages = Convert.ToInt32(
                    Math.Ceiling(
                        ((double)(banlists.Count()) / (double)(numberOfResultsPerPage))
                    )
                );

            var banlistsToActualPage = banlists
                    .Skip(numberOfResultsPerPage * (queryParams.PageNumber - 1))
                    .Take(numberOfResultsPerPage)
                    .ToList();

            var dtos = _banlistToDtoConverter.Convert(banlistsToActualPage);

            return Ok(new BanlistBrowserResultsDTO(numberOfPages, dtos));
        }
    }
}
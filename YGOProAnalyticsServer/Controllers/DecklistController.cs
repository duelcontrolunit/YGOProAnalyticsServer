using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
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

        public DecklistController(
            YgoProAnalyticsDatabase db,
            IDecklistToDecklistDtoConverter decklistToDtoConverter,
            IDecklistService decklistService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _decklistToDtoConverter = decklistToDtoConverter ?? throw new ArgumentNullException(nameof(decklistToDtoConverter));
            _decklistService = decklistService ?? throw new ArgumentNullException(nameof(decklistService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var deck = await _decklistService.GetByIdWithAllDataIncluded(id);
             
            if(deck == null)
            {
                return NotFound("There is no decklist with given id.");
            }

            var decklistDto = _decklistToDtoConverter.Convert(deck);

            return new JsonResult(decklistDto);
        }
    }
}
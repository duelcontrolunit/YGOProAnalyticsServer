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

namespace YGOProAnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecklistController : ControllerBase
    {
        readonly YgoProAnalyticsDatabase _db;
        readonly IDecklistToDecklistDtoConverter _decklistToDtoConverter;

        public DecklistController(YgoProAnalyticsDatabase db, IDecklistToDecklistDtoConverter decklistToDtoConverter)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _decklistToDtoConverter = decklistToDtoConverter ?? throw new ArgumentNullException(nameof(decklistToDtoConverter));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var deck = await _db
                  .Decklists
                  //Include main deck
                  .Include(x => x.MainDeck)
                        .ThenInclude(x => x.Archetype)
                   .Include(x => x.MainDeck)
                        .ThenInclude(x => x.MonsterCard)
                            .ThenInclude(x => x.PendulumMonsterCard)
                   .Include($"{nameof(Decklist.MainDeck)}.{Card.IncludeWithForbiddenCardsBanlist}")
                   .Include($"{nameof(Decklist.MainDeck)}.{Card.IncludeWithLimitedCardsBanlist}")
                   .Include($"{nameof(Decklist.MainDeck)}.{Card.IncludeWithSemiLimitedCardsBanlist}")
                  //Include extra deck
                  .Include(x => x.ExtraDeck)
                        .ThenInclude(x => x.Archetype)
                  .Include(x => x.ExtraDeck)
                        .ThenInclude(x => x.MonsterCard)
                            .ThenInclude(x => x.PendulumMonsterCard)
                        .ThenInclude(x => x.MonsterCard)
                            .ThenInclude(x => x.LinkMonsterCard)
                  .Include($"{nameof(Decklist.ExtraDeck)}.{Card.IncludeWithForbiddenCardsBanlist}")
                  .Include($"{nameof(Decklist.ExtraDeck)}.{Card.IncludeWithLimitedCardsBanlist}")
                  .Include($"{nameof(Decklist.ExtraDeck)}.{Card.IncludeWithSemiLimitedCardsBanlist}")
                  //Include side deck
                  .Include(x => x.SideDeck)
                        .ThenInclude(x => x.Archetype)
                   .Include(x => x.SideDeck)
                        .ThenInclude(x => x.MonsterCard)
                            .ThenInclude(x => x.PendulumMonsterCard)
                        .ThenInclude(x => x.MonsterCard)
                            .ThenInclude(x => x.LinkMonsterCard)
                  .Include($"{nameof(Decklist.SideDeck)}.{Card.IncludeWithForbiddenCardsBanlist}")
                  .Include($"{nameof(Decklist.SideDeck)}.{Card.IncludeWithLimitedCardsBanlist}")
                  .Include($"{nameof(Decklist.SideDeck)}.{Card.IncludeWithSemiLimitedCardsBanlist}")
                  //Include other properties
                  .Include(x => x.Archetype)
                  .Include(x => x.DecklistStatistics)
                  .Where(x => x.Id == id)
                  .FirstOrDefaultAsync();
               
            if(deck == null)
            {
                return NotFound("There is no decklist with given id.");
            }

            var decklistDto = _decklistToDtoConverter.Convert(deck);

            return new JsonResult(decklistDto);
        }
    }
}
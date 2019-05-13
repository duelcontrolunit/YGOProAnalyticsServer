using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Services.Others
{
    public class DecklistService : IDecklistService
    {
        YgoProAnalyticsDatabase _db;

        public DecklistService(YgoProAnalyticsDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Decklist> GetByIdWithAllDataIncluded(int id)
        {
            var query = includeMainDeckWithAllData(_db.Decklists);
            query = includeExtraDeckWithAllData(query);
            query = includeSideDeckWithAllData(query);
            query.Include(x => x.Archetype)
                 .Include(x => x.DecklistStatistics)
                 .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        protected IQueryable<Decklist> includeMainDeckWithAllData(IQueryable<Decklist> query)
        {
            return query
                .Include(x => x.MainDeck)
                    .ThenInclude(x => x.Archetype)
                .Include(x => x.MainDeck)
                    .ThenInclude(x => x.MonsterCard)
                        .ThenInclude(x => x.PendulumMonsterCard)
                .Include($"{nameof(Decklist.MainDeck)}.{Card.IncludeWithForbiddenCardsBanlist}")
                .Include($"{nameof(Decklist.MainDeck)}.{Card.IncludeWithLimitedCardsBanlist}")
                .Include($"{nameof(Decklist.MainDeck)}.{Card.IncludeWithSemiLimitedCardsBanlist}");
        }

        protected IQueryable<Decklist> includeExtraDeckWithAllData(IQueryable<Decklist> query)
        {
            return query
                .Include(x => x.ExtraDeck)
                    .ThenInclude(x => x.Archetype)
                .Include(x => x.ExtraDeck)
                    .ThenInclude(x => x.MonsterCard)
                        .ThenInclude(x => x.PendulumMonsterCard)
                    .ThenInclude(x => x.MonsterCard)
                        .ThenInclude(x => x.LinkMonsterCard)
                .Include($"{nameof(Decklist.ExtraDeck)}.{Card.IncludeWithForbiddenCardsBanlist}")
                .Include($"{nameof(Decklist.ExtraDeck)}.{Card.IncludeWithLimitedCardsBanlist}")
                .Include($"{nameof(Decklist.ExtraDeck)}.{Card.IncludeWithSemiLimitedCardsBanlist}");
        }

        protected IQueryable<Decklist> includeSideDeckWithAllData(IQueryable<Decklist> query)
        {
            return query
                   .Include(x => x.SideDeck)
                        .ThenInclude(x => x.Archetype)
                   .Include(x => x.SideDeck)
                        .ThenInclude(x => x.MonsterCard)
                            .ThenInclude(x => x.PendulumMonsterCard)
                        .ThenInclude(x => x.MonsterCard)
                            .ThenInclude(x => x.LinkMonsterCard)
                  .Include($"{nameof(Decklist.SideDeck)}.{Card.IncludeWithForbiddenCardsBanlist}")
                  .Include($"{nameof(Decklist.SideDeck)}.{Card.IncludeWithLimitedCardsBanlist}")
                  .Include($"{nameof(Decklist.SideDeck)}.{Card.IncludeWithSemiLimitedCardsBanlist}");
        }
    }
}

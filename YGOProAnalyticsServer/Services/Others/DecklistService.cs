using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DbModels.DbJoinModels;
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

            return await query.Include(x => x.Archetype)
                .Include(x => x.DecklistStatistics)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        protected IQueryable<Decklist> includeMainDeckWithAllData(IQueryable<Decklist> query)
        {
            return query
                .Include($"{Decklist.IncludeMainDeckCards}")

                .Include($"{Decklist.IncludeMainDeckCards}.{nameof(Card.Archetype)}")

                .Include($"{Decklist.IncludeMainDeckCards}" +
                         $".{nameof(Card.MonsterCard)}" +
                         $".{nameof(MonsterCard.PendulumMonsterCard)}")

                .Include($"{Decklist.IncludeMainDeckCards}" +
                         $".{Card.IncludeWithForbiddenCardsBanlist}")

                .Include($"{Decklist.IncludeMainDeckCards}" +
                         $".{Card.IncludeWithLimitedCardsBanlist}")

                .Include($"{Decklist.IncludeMainDeckCards}" +
                         $".{Card.IncludeWithSemiLimitedCardsBanlist}")
               ;
        }

        protected IQueryable<Decklist> includeExtraDeckWithAllData(IQueryable<Decklist> query)
        {
            return query
               .Include($"{Decklist.IncludeExtraDeckCards}")

               .Include($"{Decklist.IncludeExtraDeckCards}.{nameof(Card.Archetype)}")

               .Include($"{Decklist.IncludeExtraDeckCards}" +
                        $".{nameof(Card.MonsterCard)}" +
                        $".{nameof(MonsterCard.PendulumMonsterCard)}")

                .Include($"{Decklist.IncludeExtraDeckCards}" +
                        $".{nameof(Card.MonsterCard)}" +
                        $".{nameof(MonsterCard.LinkMonsterCard)}")

               .Include($"{Decklist.IncludeExtraDeckCards}" +
                        $".{Card.IncludeWithForbiddenCardsBanlist}")

               .Include($"{Decklist.IncludeExtraDeckCards}" +
                        $".{Card.IncludeWithLimitedCardsBanlist}")

               .Include($"{Decklist.IncludeExtraDeckCards}" +
                        $".{Card.IncludeWithSemiLimitedCardsBanlist}")
              ;
        }

        protected IQueryable<Decklist> includeSideDeckWithAllData(IQueryable<Decklist> query)
        {
            return query
              .Include($"{Decklist.IncludeSideDeckCards}")

              .Include($"{Decklist.IncludeSideDeckCards}.{nameof(Card.Archetype)}")

              .Include($"{Decklist.IncludeSideDeckCards}" +
                       $".{nameof(Card.MonsterCard)}" +
                       $".{nameof(MonsterCard.PendulumMonsterCard)}")

                .Include($"{Decklist.IncludeSideDeckCards}" +
                        $".{nameof(Card.MonsterCard)}" +
                        $".{nameof(MonsterCard.LinkMonsterCard)}")

              .Include($"{Decklist.IncludeSideDeckCards}" +
                       $".{Card.IncludeWithForbiddenCardsBanlist}")

              .Include($"{Decklist.IncludeSideDeckCards}" +
                       $".{Card.IncludeWithLimitedCardsBanlist}")

              .Include($"{Decklist.IncludeSideDeckCards}" +
                       $".{Card.IncludeWithSemiLimitedCardsBanlist}")
             ;
        }
    }
}

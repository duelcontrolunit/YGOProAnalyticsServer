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
                .Include($"{nameof(Decklist.CardsInMainDeckJoin)}" +
                         $".{nameof(CardInMainDeckDecklistJoin.Card)}")

                .Include($"{nameof(Decklist.CardsInMainDeckJoin)}" +
                         $".{nameof(CardInMainDeckDecklistJoin.Card)}" +
                         $".{nameof(Card.Archetype)}")

                .Include($"{nameof(Decklist.CardsInMainDeckJoin)}" +
                         $".{nameof(CardInMainDeckDecklistJoin.Card)}" +
                         $".{nameof(Card.MonsterCard)}" +
                         $".{nameof(MonsterCard.PendulumMonsterCard)}")

                .Include($"{nameof(Decklist.CardsInMainDeckJoin)}" +
                         $".{nameof(CardInMainDeckDecklistJoin.Card)}" +
                         $".{Card.IncludeWithForbiddenCardsBanlist}")

                .Include($"{nameof(Decklist.CardsInMainDeckJoin)}" +
                         $".{nameof(CardInMainDeckDecklistJoin.Card)}" +
                         $".{Card.IncludeWithLimitedCardsBanlist}")

                .Include($"{nameof(Decklist.CardsInMainDeckJoin)}" +
                         $".{nameof(CardInMainDeckDecklistJoin.Card)}" +
                         $".{Card.IncludeWithSemiLimitedCardsBanlist}")
               ;
        }

        protected IQueryable<Decklist> includeExtraDeckWithAllData(IQueryable<Decklist> query)
        {
            return query
                 .Include($"{nameof(Decklist.CardsInExtraDeckJoin)}" +
                          $".{nameof(CardInExtraDeckDecklistJoin.Card)}")

                 .Include($"{nameof(Decklist.CardsInExtraDeckJoin)}" +
                          $".{nameof(CardInExtraDeckDecklistJoin.Card)}" +
                          $".{nameof(Card.Archetype)}")

                 .Include($"{nameof(Decklist.CardsInExtraDeckJoin)}" +
                          $".{nameof(CardInExtraDeckDecklistJoin.Card)}" +
                          $".{nameof(Card.MonsterCard)}" +
                          $".{nameof(MonsterCard.PendulumMonsterCard)}")

                 .Include($"{nameof(Decklist.CardsInExtraDeckJoin)}" +
                          $".{nameof(CardInExtraDeckDecklistJoin.Card)}" +
                          $".{nameof(Card.MonsterCard)}" +
                          $".{nameof(MonsterCard.LinkMonsterCard)}")

                 .Include($"{nameof(Decklist.CardsInExtraDeckJoin)}" +
                          $".{nameof(CardInExtraDeckDecklistJoin.Card)}" +
                          $".{Card.IncludeWithForbiddenCardsBanlist}")

                 .Include($"{nameof(Decklist.CardsInExtraDeckJoin)}" +
                          $".{nameof(CardInExtraDeckDecklistJoin.Card)}" +
                          $".{Card.IncludeWithLimitedCardsBanlist}")

                 .Include($"{nameof(Decklist.CardsInExtraDeckJoin)}" +
                          $".{nameof(CardInExtraDeckDecklistJoin.Card)}" +
                          $".{Card.IncludeWithSemiLimitedCardsBanlist}")
                ;
        }

        protected IQueryable<Decklist> includeSideDeckWithAllData(IQueryable<Decklist> query)
        {
            return query
                 .Include($"{nameof(Decklist.CardsInSideDeckJoin)}" +
                          $".{nameof(CardInSideDeckDecklistJoin.Card)}")

                 .Include($"{nameof(Decklist.CardsInSideDeckJoin)}" +
                          $".{nameof(CardInSideDeckDecklistJoin.Card)}" +
                          $".{nameof(Card.Archetype)}")

                 .Include($"{nameof(Decklist.CardsInSideDeckJoin)}" +
                          $".{nameof(CardInSideDeckDecklistJoin.Card)}" +
                          $".{nameof(Card.MonsterCard)}" +
                          $".{nameof(MonsterCard.PendulumMonsterCard)}")

                 .Include($"{nameof(Decklist.CardsInExtraDeckJoin)}" +
                          $".{nameof(CardInExtraDeckDecklistJoin.Card)}" +
                          $".{nameof(Card.MonsterCard)}" +
                          $".{nameof(MonsterCard.LinkMonsterCard)}")

                 .Include($"{nameof(Decklist.CardsInSideDeckJoin)}" +
                          $".{nameof(CardInSideDeckDecklistJoin.Card)}" +
                          $".{Card.IncludeWithForbiddenCardsBanlist}")

                 .Include($"{nameof(Decklist.CardsInSideDeckJoin)}" +
                          $".{nameof(CardInSideDeckDecklistJoin.Card)}" +
                          $".{Card.IncludeWithLimitedCardsBanlist}")

                 .Include($"{nameof(Decklist.CardsInSideDeckJoin)}" +
                          $".{nameof(CardInSideDeckDecklistJoin.Card)}" +
                          $".{Card.IncludeWithSemiLimitedCardsBanlist}")
                ;
        }
    }
}

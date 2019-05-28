using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        readonly YgoProAnalyticsDatabase _db;
        readonly IBanlistService _banlistService;

        public DecklistService(YgoProAnalyticsDatabase db, IBanlistService banlistService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _banlistService = banlistService ?? throw new ArgumentNullException(nameof(banlistService));
        }

        public async Task<IEnumerable<Decklist>> Get(
            int howManyTake,
            int howManySkip,
            int banlistId)
        {
            var query = _getDecklistsQuery();
            if (banlistId != -1)
            {
                Banlist banlist = await _db
                    .Banlists
                    .Where(x => x.Id == banlistId)
                    .Include(Banlist.IncludeWithForbiddenCards)
                    .Include(Banlist.IncludeWithLimitedCards)
                    .Include(Banlist.IncludeWithSemiLimitedCards)
                    .FirstOrDefaultAsync();

                if(banlist != null)
                {
                    int skipMultiplicator = 1;
                    bool notAllDecksWereChecked = true;
                    var decksForReturn = new List<Decklist>();
                    do
                    {
                        var decks = await query
                            .Skip(howManySkip * skipMultiplicator)
                            .Take(howManyTake)
                            .ToListAsync();

                        foreach (var decklist in decks)
                        {
                            if (_banlistService.CanDeckBeUsedOnGivenBanlist(decklist, banlist)
                                && decksForReturn.Count < 100)
                            {
                                decksForReturn.Add(decklist);
                            }
                        }

                        if (decks.Count == 0)
                        {
                            notAllDecksWereChecked = false;
                        }

                        skipMultiplicator++;
                    } while (decksForReturn.Count < 100 && notAllDecksWereChecked);

                    return decksForReturn;
                }
                else
                {
                    return new List<Decklist>();
                }
            }
            else
            {
                return await query
                    .Skip(howManySkip)
                    .Take(howManyTake)
                    .ToListAsync();
            }
        }

        public async Task<Decklist> GetByIdWithAllDataIncluded(int id)
        {
            var query = _getDecklistsQuery();

            return await query
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        private IQueryable<Decklist> _getDecklistsQuery()
        {
            var query = includeMainDeckWithAllData(_db.Decklists);
            query = includeExtraDeckWithAllData(query);
            query = includeSideDeckWithAllData(query);
            query = query.Include(x => x.Archetype);
            query = query.Include(x => x.DecklistStatistics);

            return query;
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

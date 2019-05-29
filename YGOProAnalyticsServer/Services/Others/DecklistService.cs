using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DbModels.DbJoinModels;
using YGOProAnalyticsServer.Helpers;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Services.Others
{
    public class DecklistService : IDecklistService
    {
        readonly YgoProAnalyticsDatabase _db;
        readonly IBanlistService _banlistService;
        readonly IMemoryCache _cache;

        public DecklistService(
            YgoProAnalyticsDatabase db,
            IBanlistService banlistService,
            IMemoryCache cache)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _banlistService = banlistService ?? throw new ArgumentNullException(nameof(banlistService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Decklist>> FindAll(
            int howManyTake,
            int howManySkip,
            int minNumberOfGames = 10,
            int banlistId = -1,
            string archetypeName = "",
            bool shouldGetDecksFromCache = true)
        {
            IEnumerable<Decklist> localDecklistsQuery = _getDecklistQueryAsEnumerable(shouldGetDecksFromCache);
            localDecklistsQuery = localDecklistsQuery.Where(
                x => x.DecklistStatistics.Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames
            );

            if (!string.IsNullOrEmpty(archetypeName))
            {
                localDecklistsQuery = localDecklistsQuery.Where(x => x.Archetype.Name.Contains(archetypeName));
            }

            if (banlistId > 0)
            {
                var banlist = await _banlistService
                    .GetBanlistWithAllCardsIncludedAsync(banlistId);
                if (banlist != null)
                {
                    localDecklistsQuery = localDecklistsQuery
                        .Where(x => _banlistService.CanDeckBeUsedOnGivenBanlist(x, banlist));
                }
            }

            return localDecklistsQuery
                    .Skip(howManySkip)
                    .Take(howManyTake)
                    .ToList();
        }

        private IEnumerable<Decklist> _getDecklistQueryAsEnumerable(bool shouldGetDecksFromCache)
        {
            IEnumerable<Decklist> localDecklistsQuery;
            if (!shouldGetDecksFromCache)
            {
                localDecklistsQuery = _getOrderedDecklistsAsEnumerable();
            }
            else if (!_cache.TryGetValue(CacheKeys.OrderedDecklistsWithContentIncluded, out localDecklistsQuery))
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.High)
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                localDecklistsQuery = _getOrderedDecklistsAsEnumerable();
                _cache.Set("", localDecklistsQuery, cacheOptions);
            }

            return localDecklistsQuery;
        }

        private IEnumerable<Decklist> _getOrderedDecklistsAsEnumerable()
        {
            return _getDecklistsQuery()
                .OrderByDescending(
                    x => x.DecklistStatistics.Sum(y => y.NumberOfTimesWhenDeckWon)
                 )
                 .AsEnumerable();
        }

        /// <summary>
        /// Compares the by win ratio.
        /// </summary>
        /// <param name="firstDecklist">The first decklist.</param>
        /// <param name="secondDecklist">The second decklist.</param>
        /// <returns></returns>
        public int CompareByWinRatio(
            Decklist firstDecklist,
            Decklist secondDecklist)
        {
            if(GetWinRatio(firstDecklist) > GetWinRatio(secondDecklist))
            {
                return 1;
            }
            else if(GetWinRatio(firstDecklist) == GetWinRatio(secondDecklist))
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        public int GetWinRatio(Decklist decklist)
        {
            return 4;
        }

        /// <inheritdoc />
        public async Task<Decklist> GetByIdWithAllDataIncluded(int id)
        {
            var query = _getDecklistsQuery();

            return await query
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the decklists query with all data explicitly included.
        /// </summary>
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

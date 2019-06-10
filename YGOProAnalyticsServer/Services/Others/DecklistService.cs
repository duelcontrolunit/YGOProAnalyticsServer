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
    /// <summary>
    /// Provide features related with decklists.
    /// </summary>
    public class DecklistService : IDecklistService
    {
        readonly YgoProAnalyticsDatabase _db;
        readonly IBanlistService _banlistService;
        readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecklistService"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="banlistService">The banlist service.</param>
        /// <param name="cache">The cache.</param>
        /// <exception cref="ArgumentNullException">
        /// db
        /// or
        /// banlistService
        /// or
        /// cache
        /// </exception>
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
            DateTime? statisticsFrom = null,
            DateTime? statisticsTo = null,
            bool shouldGetDecksFromCache = true)
        {
            IEnumerable<Decklist> localDecklistsQuery = await _getOrCreateAndGetOrderedDecklistFromCache(shouldGetDecksFromCache);
            if(statisticsTo == null && statisticsFrom == null)
            {
                localDecklistsQuery = _addMinNumberOfGamesFilterToLocalDecklistQuery(minNumberOfGames, localDecklistsQuery);
            }
            else
            {
                localDecklistsQuery = _addStatisticsDateLimitIfRequiredAndThenMinNumberOfGamesFilter(
                  statisticsFrom,
                  statisticsTo,
                  minNumberOfGames,
                  localDecklistsQuery);

                localDecklistsQuery = _orderUsingDateLimitAndMinNumberOfGamesCriteria(
                  statisticsFrom,
                  statisticsTo,
                  minNumberOfGames,
                  localDecklistsQuery);
            }
          
            localDecklistsQuery = _addArchetypeNameFilterToLocalDecklistQueryIfRequired(archetypeName, localDecklistsQuery);
            localDecklistsQuery = await _addBanlistFilterToLocalDecklistQueryIfRequired(banlistId, localDecklistsQuery);

            return localDecklistsQuery
                    .Skip(howManySkip)
                    .Take(howManyTake)
                    .ToList();
        }

        private IEnumerable<Decklist> _orderUsingDateLimitAndMinNumberOfGamesCriteria(
            DateTime? statisticsFrom,
            DateTime? statisticsTo,
            int minNumberOfGames,
            IEnumerable<Decklist> localDecklistsQuery)
        {
           if (statisticsFrom != null && statisticsTo == null)
           {
                localDecklistsQuery = localDecklistsQuery
                    .OrderByDescending(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom)
                        .Sum(y => y.NumberOfTimesWhenDeckWon)
                 );
           }
           else
           if (statisticsTo != null && statisticsFrom == null)
           {
                localDecklistsQuery = localDecklistsQuery
                    .OrderByDescending(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWon)
                );
           }
           else
           if (statisticsFrom != null && statisticsTo != null)
           {
                localDecklistsQuery = localDecklistsQuery
                     .OrderByDescending(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom
                               && z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWon)
                );
           }

           return localDecklistsQuery;
        }

        private IEnumerable<Decklist> _addStatisticsDateLimitIfRequiredAndThenMinNumberOfGamesFilter(
            DateTime? statisticsFrom,
            DateTime? statisticsTo,
            int minNumberOfGames,
            IEnumerable<Decklist> localDecklistsQuery)
        {

            if(statisticsFrom != null && statisticsTo == null)
            {
                localDecklistsQuery = localDecklistsQuery
                     .Where(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames
                );
            }
            else
            if(statisticsTo != null && statisticsFrom == null)
            {
                localDecklistsQuery = localDecklistsQuery
                     .Where(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames
                );
            }
            else
            if(statisticsFrom != null && statisticsTo != null)
            {
                localDecklistsQuery = localDecklistsQuery
                     .Where(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom
                               && z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames
                );
            }

            return localDecklistsQuery;
        }

        private async Task<IEnumerable<Decklist>> _addBanlistFilterToLocalDecklistQueryIfRequired(
            int banlistId,
            IEnumerable<Decklist> localDecklistsQuery)
        {
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

            return localDecklistsQuery;
        }

        private static IEnumerable<Decklist> _addArchetypeNameFilterToLocalDecklistQueryIfRequired(
            string archetypeName,
            IEnumerable<Decklist> localDecklistsQuery)
        {
            if (!string.IsNullOrEmpty(archetypeName))
            {
                localDecklistsQuery = localDecklistsQuery.Where(x => x.Archetype.Name.Contains(archetypeName));
            }

            return localDecklistsQuery;
        }

        private IEnumerable<Decklist> _addMinNumberOfGamesFilterToLocalDecklistQuery(
            int minNumberOfGames,
            IEnumerable<Decklist> localDecklistsQuery)
        {
            localDecklistsQuery = localDecklistsQuery.Where(
                x => x.DecklistStatistics.Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames
            );

            return localDecklistsQuery;
        }

        /// <inheritdoc />
        public async Task UpdateCache()
        {
            _cache.Remove(CacheKeys.OrderedDecklistsWithContentIncluded);
            await _getOrCreateAndGetOrderedDecklistFromCache(true);
        }

        private async Task<IEnumerable<Decklist>> _getOrCreateAndGetOrderedDecklistFromCache(bool shouldGetDecksFromCache)
        {
            IEnumerable<Decklist> localDecklistsQuery;
            if (!shouldGetDecksFromCache)
            {
                localDecklistsQuery = await _getOrderedNoTrackedDecklists();
            }
            else if (!_cache.TryGetValue(CacheKeys.OrderedDecklistsWithContentIncluded, out localDecklistsQuery))
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    //It is really important, because If there is no decklists in cache,
                    //it is easy to achieve OutOfMemoryException.
                    //In context of YGOProAnalyticsServer project
                    //this entry should be updated only by 
                    //UpdateCache() method directly after analysis process (When entire API is still blocked).
                    .SetPriority(CacheItemPriority.NeverRemove);

                localDecklistsQuery = await _getOrderedNoTrackedDecklists();
                _cache.Set(CacheKeys.OrderedDecklistsWithContentIncluded, localDecklistsQuery, cacheOptions);
            }

            return localDecklistsQuery;
        }

        private async Task<List<Decklist>> _getOrderedNoTrackedDecklists()
        {
            _db.Database.SetCommandTimeout(3600);
            return await _getDecklistsQuery(false)
                .OrderByDescending(
                    x => x.DecklistStatistics.Sum(y => y.NumberOfTimesWhenDeckWon)
                 ).AsNoTracking()
                 .ToListAsync();
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
        private IQueryable<Decklist> _getDecklistsQuery(bool shouldBeTrackd = true)
        {
            IQueryable<Decklist> query;
            if (shouldBeTrackd)
            {
                query = includeMainDeckWithAllData(_db.Decklists);
            }
            else
            {
                query = includeMainDeckWithAllData(_db.Decklists.AsNoTracking());
            }
           
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

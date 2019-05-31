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
            DateTime? statisticsFrom = null,
            DateTime? statisticsTo = null,
            bool shouldGetDecksFromCache = true)
        {
            IEnumerable<Decklist> localDecklistsQuery = _getOrCreateAndGetDecklistFromCache(shouldGetDecksFromCache);
            localDecklistsQuery = _addMinNumberOfGamesFilterToLocalDecklistQuery(minNumberOfGames, localDecklistsQuery);
            localDecklistsQuery = _addArchetypeNameFilterToLocalDecklistQueryIfRequired(archetypeName, localDecklistsQuery);
            localDecklistsQuery = await _addBanlistFilterToLocalDecklistQueryIfRequired(banlistId, localDecklistsQuery);

            return localDecklistsQuery
                    .Skip(howManySkip)
                    .Take(howManyTake)
                    .ToList();
        }

        private async Task<IEnumerable<Decklist>> _addBanlistFilterToLocalDecklistQueryIfRequired(int banlistId, IEnumerable<Decklist> localDecklistsQuery)
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
        public void UpdateCache()
        {
            _cache.Remove(CacheKeys.OrderedDecklistsWithContentIncluded);
            _getOrCreateAndGetDecklistFromCache(true);
        }

        private IEnumerable<Decklist> _getOrCreateAndGetDecklistFromCache(bool shouldGetDecksFromCache)
        {
            IEnumerable<Decklist> localDecklistsQuery;
            if (!shouldGetDecksFromCache)
            {
                localDecklistsQuery = _getOrderedNoTrackedDecklists();
            }
            else if (!_cache.TryGetValue(CacheKeys.OrderedDecklistsWithContentIncluded, out localDecklistsQuery))
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.High)
                    .SetSlidingExpiration(TimeSpan.FromHours(23));

                localDecklistsQuery = _getOrderedNoTrackedDecklists();
                _cache.Set(CacheKeys.OrderedDecklistsWithContentIncluded, localDecklistsQuery, cacheOptions);
            }

            return localDecklistsQuery;
        }

        private List<Decklist> _getOrderedNoTrackedDecklists()
        {
            _db.Database.SetCommandTimeout(3600);
            return _getDecklistsQuery(false)
                .OrderByDescending(
                    x => x.DecklistStatistics.Sum(y => y.NumberOfTimesWhenDeckWon)
                 )
                 .ToList();
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

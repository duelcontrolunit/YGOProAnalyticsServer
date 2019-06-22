using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using Microsoft.EntityFrameworkCore;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Others.Interfaces;
using YGOProAnalyticsServer.Helpers;
using Microsoft.Extensions.Caching.Memory;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Others
{
    /// <summary>
    /// Provide features related with archetypes.
    /// </summary>
    public class ArchetypeService : IArchetypeService
    {
        readonly YgoProAnalyticsDatabase _db;
        readonly IMemoryCache _cache;
        readonly IAdminConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchetypeService"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="config">The configuration.</param>
        /// <exception cref="ArgumentNullException">
        /// db
        /// or
        /// cache
        /// or
        /// config
        /// </exception>
        public ArchetypeService(YgoProAnalyticsDatabase db, IMemoryCache cache, IAdminConfig config)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ArchetypeIdAndNameDTO>> GetPureArchetypeListWithIdsAndNamesAsNoTrackingFromCache(bool shouldIgnoreCache)
        {
            IEnumerable<ArchetypeIdAndNameDTO> dtos;
            if (shouldIgnoreCache)
            {
                return await _getPureArchetypeIdAndNameDtosAsNoTracking();
            }
            else
            if (!_cache.TryGetValue(CacheKeys.ArchetypeIdAndNameDtos, out dtos))
            {
                dtos = await _getPureArchetypeIdAndNameDtosAsNoTracking();
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSlidingExpiration(
                        TimeSpan.FromHours(_config.ArchetypeSlidingCacheExpirationInHours))
                    .SetAbsoluteExpiration(
                        TimeSpan.FromHours(_config.ArchetypeAbsoluteCacheExpirationInHours));

                _cache.Set(CacheKeys.ArchetypeIdAndNameDtos, dtos, cacheOptions);
            }

            return dtos;
        }

        private async Task<IEnumerable<ArchetypeIdAndNameDTO>> _getPureArchetypeIdAndNameDtosAsNoTracking()
        {
            return await _db
               .Archetypes
               .AsNoTracking()
               .Where(x => x.IsPureArchetype == true)
               .Select(x => new ArchetypeIdAndNameDTO(x.Id, x.Name))
               .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IQueryable<Archetype>> FindAllQuery(
            int minNumberOfGames,
            string archetypeName = "",
            DateTime? statisticsFromDate = null,
            DateTime? statisticsToDate = null,
            bool includeCards = false,
            bool includeDecks = false)
        {
            IQueryable<Archetype> archetypesQuery = _db.Archetypes.Include(x => x.Statistics);
            if (includeCards)
            {
                archetypesQuery = archetypesQuery.Include(x => x.Cards);
            }

            if (includeDecks)
            {
                archetypesQuery = archetypesQuery.Include(x => x.Decklists);
            }

            archetypesQuery = archetypesQuery.Where(x => x.Name.ToLower().Contains(archetypeName.ToLower()));
            if (statisticsFromDate != null && statisticsToDate == null)
            {
                return archetypesQuery
                    .Where(x => x.
                        Statistics
                            .Where(y => y.DateWhenArchetypeWasUsed >= statisticsFromDate)
                            .Sum(y => y.NumberOfDecksWhereWasUsed) >= minNumberOfGames
                    )
                    .OrderByDescending(x => x.Statistics
                        .Where(y => y.DateWhenArchetypeWasUsed >= statisticsFromDate)
                        .Sum(y => y.NumberOfTimesWhenArchetypeWon)
                     );
            }
            else
            if (statisticsFromDate == null && statisticsToDate != null)
            {
                return archetypesQuery
                    .Where(x => x.
                        Statistics
                            .Where(y => y.DateWhenArchetypeWasUsed <= statisticsToDate)
                            .Sum(y => y.NumberOfDecksWhereWasUsed) >= minNumberOfGames
                    )
                     .OrderByDescending(x => x.Statistics
                        .Where(y => y.DateWhenArchetypeWasUsed <= statisticsToDate)
                        .Sum(y => y.NumberOfTimesWhenArchetypeWon)
                     );
            }
            else
            if (statisticsFromDate != null && statisticsToDate != null)
            {
                return archetypesQuery
                    .Where(x => x.
                        Statistics
                            .Where(y => y.DateWhenArchetypeWasUsed <= statisticsToDate
                                     && y.DateWhenArchetypeWasUsed >= statisticsFromDate)
                            .Sum(y => y.NumberOfDecksWhereWasUsed) >= minNumberOfGames
                    )
                    .OrderByDescending(x => x.Statistics
                        .Where(y => y.DateWhenArchetypeWasUsed <= statisticsToDate
                                 && y.DateWhenArchetypeWasUsed >= statisticsFromDate)
                        .Sum(y => y.NumberOfTimesWhenArchetypeWon)
                    );
            }
            else
            {
                return archetypesQuery
                    .Where(x => x.Statistics.Sum(y => y.NumberOfDecksWhereWasUsed) >= minNumberOfGames)
                     .OrderByDescending(x => x.Statistics
                       .Sum(y => y.NumberOfTimesWhenArchetypeWon)
                    );
            }
        }

        /// <inheritdoc />
        public async Task<Archetype> GetDataForConcreteArchetypePage(int id)
        {
            return await _db
                .Archetypes
                .Include(x => x.Statistics)
                .Include(x => x.Cards)
                    .ThenInclude(x => x.MonsterCard)
                .Include(x => x.Cards)
                    .ThenInclude(x => x.MonsterCard)
                        .ThenInclude(x => x.LinkMonsterCard)
                .Include(x => x.Cards)
                    .ThenInclude(x => x.MonsterCard)
                        .ThenInclude(x => x.PendulumMonsterCard)
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}

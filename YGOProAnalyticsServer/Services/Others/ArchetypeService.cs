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
                return await _getArchetypeIdAndNameDtosAsNoTracking();
            }
            else
            if (!_cache.TryGetValue(CacheKeys.ArchetypeIdAndNameDtos, out dtos))
            {
                dtos = await _getArchetypeIdAndNameDtosAsNoTracking();
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

        private async Task<IEnumerable<ArchetypeIdAndNameDTO>> _getArchetypeIdAndNameDtosAsNoTracking()
        {
            return await _db
               .Archetypes
               .AsNoTracking()
               .Where(x => x.IsPureArchetype == true)
               .Select(x => new ArchetypeIdAndNameDTO(x.Id, x.Name))
               .ToListAsync();
        }
    }
}

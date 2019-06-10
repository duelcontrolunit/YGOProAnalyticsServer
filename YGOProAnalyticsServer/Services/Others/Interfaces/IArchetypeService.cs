using System.Collections.Generic;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Others.Interfaces
{
    /// <summary>
    /// Provide features related with archetypes.
    /// </summary>
    public interface IArchetypeService
    {
        /// <summary>
        /// Gets the archetype list with ids and names as no tracking from cache.
        /// </summary>
        /// <param name="shouldIgnoreCache">Set true to ignore cache.</param>
        /// <returns>Archetype list with ids and names as no tracking from cache.</returns>
        Task<IEnumerable<ArchetypeIdAndNameDTO>> GetPureArchetypeListWithIdsAndNamesAsNoTrackingFromCache(bool shouldIgnoreCache = false);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Others.Interfaces
{
    /// <summary>
    /// Provide features related with archetypes.
    /// </summary>
    public interface IArchetypeService
    {
        /// <summary>
        /// Get query based on parameters. Results by default are sorted by number of wins.
        /// </summary>
        /// <param name="minNumberOfGames">The minimum number of games.</param>
        /// <param name="archetypeName">Name of the archetype.</param>
        /// <param name="statisticsFromDate">Use statistics from date.</param>
        /// <param name="statisticsToDate">Use statistics to date.</param>
        /// <param name="includeCards">Include cards?</param>
        /// <param name="includeDecks">Include decklists?</param>
        Task<IQueryable<Archetype>> FindAllQuery(
            int minNumberOfGames,
            string archetypeName = "",
            DateTime? statisticsFromDate = null,
            DateTime? statisticsToDate = null,
            bool includeCards = false,
            bool includeDecks = false);

        /// <summary>
        /// Gets the archetype list with ids and names as no tracking from cache.
        /// </summary>
        /// <param name="shouldIgnoreCache">Set true to ignore cache.</param>
        /// <returns>Archetype list with ids and names as no tracking from cache.</returns>
        Task<IEnumerable<ArchetypeIdAndNameDTO>> GetPureArchetypeListWithIdsAndNamesAsNoTrackingFromCache(bool shouldIgnoreCache = false);

        /// <summary>
        /// Warning! This data are not tracked by EF.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Archetype> GetDataForConcreteArchetypePage(int id);
    }
}
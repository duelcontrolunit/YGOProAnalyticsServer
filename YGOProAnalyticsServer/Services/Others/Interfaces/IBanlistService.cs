using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Others.Interfaces
{
    /// <summary>
    /// Provide features related with banlists.
    /// </summary>
    public interface IBanlistService
    {
        /// <summary>
        /// It provide information if decklist can be used on given banlist.
        /// </summary>
        /// <param name="decklist">The decklist.</param>
        /// <param name="banlist">The banlist.</param>
        /// <returns>
        ///  Information if decklist can be used on given banlist.
        /// </returns>
        bool CanDeckBeUsedOnGivenBanlist(Decklist decklist, Banlist banlist);

        /// <summary>
        /// Gets the banlist with all cards included.
        /// </summary>
        /// <param name="banlistId">The banlist identifier.</param>
        /// <returns>Banlist with all cards included.</returns>
        Task<Banlist> GetBanlistWithAllCardsIncludedAsync(int banlistId);
        Task<IEnumerable<BanlistIdAndNameDTO>> GetListOfBanlistsNamesAndIdsAsNoTrackingFromCache(bool shouldIgnoreCache = false);

        /// <summary>
        /// Finds all query.
        /// </summary>
        /// <param name="minNumberOfGames">The minimum number of games. Banlists which have no at least that number of games will be ignored.</param>
        /// <param name="statisticsFromDate">The statistics from date.</param>
        /// <param name="statisticsToDate">The statistics to date.</param>
        Task<IQueryable<Banlist>> FindAllQuery(int minNumberOfGames, DateTime? statisticsFromDate, DateTime? statisticsToDate);
    }
}
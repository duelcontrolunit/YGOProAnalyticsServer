using System.Collections.Generic;
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
    }
}
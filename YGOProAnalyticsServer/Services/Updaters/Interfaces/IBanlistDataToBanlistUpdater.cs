using System.Collections.Generic;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Updaters.Interfaces
{
    /// <summary>
    /// Provide methods which enable banlists update.
    /// </summary>
    public interface IBanlistDataToBanlistUpdater
    {
        /// <summary>
        /// Update banlist based on data from url.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        Task<IEnumerable<Banlist>> UpdateBanlists(string url);
    }
}
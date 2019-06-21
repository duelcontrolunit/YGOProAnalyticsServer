using System.Collections.Generic;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Models;
using Microsoft.EntityFrameworkCore;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Updaters.Interfaces
{
    /// <summary>
    /// Is responsible for creating and updating <see cref="ServerActivityStatistics"/>.
    /// </summary>
    public interface IServerActivityUpdater
    {
        /// <summary>
        /// It should call <see cref="UpdateWithoutSavingChanges(IEnumerable{DuelLog})"/> 
        /// and then <see cref="DbContext.SaveChangesAsync(System.Threading.CancellationToken)"/>
        /// </summary>
        /// <param name="duelLogsFromOneFile">The duel logs from one file.</param>
        Task UpdateAndSaveChanges(IEnumerable<DuelLog> duelLogsFromOneFile);

        /// <summary>
        /// Updates <see cref="ServerActivityStatistics"/>, but without saving changes.
        /// </summary>
        /// <param name="duelLogsFromOneFile">The duel logs from one file.</param>
        Task UpdateWithoutSavingChanges(IEnumerable<DuelLog> duelLogsFromOneFile);
    }
}
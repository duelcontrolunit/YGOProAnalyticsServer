using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Models;
using YGOProAnalyticsServer.Services.Updaters.Interfaces;

namespace YGOProAnalyticsServer.Services.Updaters
{
    /// <summary>
    /// Is responsible for creating and updating <see cref="ServerActivityStatistics"/>.
    /// </summary>
    /// <seealso cref="YGOProAnalyticsServer.Services.Updaters.Interfaces.IServerActivityUpdater" />
    public class ServerActivityUpdater : IServerActivityUpdater
    {
        //Not DI dependencies
        IEnumerable<ServerActivityStatistics> _serverActivityStatistics;

        //DI dependencies
        readonly YgoProAnalyticsDatabase _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerActivityUpdater"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <exception cref="ArgumentNullException">db</exception>
        public ServerActivityUpdater(YgoProAnalyticsDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <inheritdoc />
        public async Task UpdateWithoutSavingChanges(IEnumerable<DuelLog> duelLogsFromOneFile)
        {
            var duelLogsFromDate = _sortByDate(duelLogsFromOneFile);
            await _initServerActivityStatisticsFromDbIfIsNull();
            foreach (var duelLogFromDate in duelLogsFromDate)
            {
                var fromDate = duelLogFromDate.Key;
                var duelLogs = duelLogFromDate.Value;

                var serverActivityStatistics = _serverActivityStatistics
                    .Where(x => x.FromDate.Date == fromDate.Date)
                    .FirstOrDefault();
                if(serverActivityStatistics == null)
                {
                    serverActivityStatistics = new ServerActivityStatistics(fromDate.Date);
                    serverActivityStatistics.NumberOfGames += duelLogs.Count();
                    _db.ServerActivityStatistics.Add(serverActivityStatistics);
                }
                else
                {
                    serverActivityStatistics.NumberOfGames += duelLogs.Count();
                }
            }
        }

        private Dictionary<DateTime, List<DuelLog>> _sortByDate(IEnumerable<DuelLog> duelLogsFromOneFile)
        {
            var duelLogsFromDate = new Dictionary<DateTime, List<DuelLog>>();
            foreach (var duelLog in duelLogsFromOneFile)
            {
                if (duelLogsFromDate.ContainsKey(duelLog.DateOfTheBeginningOfTheDuel.Date))
                {
                    duelLogsFromDate[duelLog.DateOfTheBeginningOfTheDuel.Date].Add(duelLog);
                }
                else
                {
                    duelLogsFromDate.Add(
                        duelLog.DateOfTheBeginningOfTheDuel.Date,
                        new List<DuelLog>() { duelLog });
                }
            }

            return duelLogsFromDate;
        }

        /// <inheritdoc />
        public async Task UpdateAndSaveChanges(IEnumerable<DuelLog> duelLogsFromOneFile)
        {
            await UpdateWithoutSavingChanges(duelLogsFromOneFile);
            await _db.SaveChangesAsync();
        }

        private async Task _initServerActivityStatisticsFromDbIfIsNull()
        {
            if(_serverActivityStatistics == null)
            {
                _serverActivityStatistics = await _db.ServerActivityStatistics.ToListAsync();
            }
        }
    }
}

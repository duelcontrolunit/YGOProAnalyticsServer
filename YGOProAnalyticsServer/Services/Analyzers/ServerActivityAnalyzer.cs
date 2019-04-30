using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.Models;
using Microsoft.EntityFrameworkCore;
using YGOProAnalyticsServer.Services.Analyzers.Interfaces;

namespace YGOProAnalyticsServer.Services.Analyzers
{
    /// <summary>
    /// Analyze server activity.
    /// </summary>
    public class ServerActivityAnalyzer : IServerActivityAnalyzer
    {
        readonly YgoProAnalyticsDatabase _db;

        /// <summary>
        /// Create new instance of <see cref="ServerActivityAnalyzer"/>
        /// </summary>
        /// <param name="db">DbContext</param>
        public ServerActivityAnalyzer(YgoProAnalyticsDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <inheritdoc />
        public int NumberOfGamesFromOneDay(IEnumerable<DuelLog> duelLogsFromOneDay)
        {
            return duelLogsFromOneDay.Count();
        }

        /// <inheritdoc />
        public async Task<int> NumberOfGamesFromOneDayAsync(DateTime date)
        {
            return await _db.ServerActivityStatistics
                .Where(
                    x => x.FromDate.Year == date.Year
                    && x.FromDate.Month == date.Month
                    && x.FromDate.Day == date.Day
                )
                .Select(x => x.NumberOfGames)
                .DefaultIfEmpty(0)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<int> NumberOfGamesFromMonthAsync(int year, int month)
        {
            return await _db.ServerActivityStatistics
                .Where(
                    x => x.FromDate.Year == year
                    && x.FromDate.Month == month
                )
                .Select(x => x.NumberOfGames)
                .DefaultIfEmpty(0)
                .SumAsync();
        }

        /// <inheritdoc />
        public async Task<int> NumberOfGamesFromLastDaysAsync(int numberOfDays)
        {
            int numberOfRecords = numberOfDays;
            var activityStatistics = await _db
                .ServerActivityStatistics
                .OrderByDescending(x => x.FromDate)
                .Take(numberOfRecords)
                .ToListAsync();

            DateTime lastDayWhichShouldBeAnalyzed = DateTime.Now.AddDays(-numberOfDays - 1);
            int numberOfGames = 0;
            foreach (var statistics in activityStatistics)
            {
                if (statistics.FromDate < lastDayWhichShouldBeAnalyzed) break;

                numberOfGames += statistics.NumberOfGames;
            }

            return numberOfGames;
        }

        /// <inheritdoc />
        public async Task<int> NumberOfGamesFromOneYearAsync(int year)
        {
            return await _db.ServerActivityStatistics
                .Where(x => x.FromDate.Year == year)
                .Select(x => x.NumberOfGames)
                .DefaultIfEmpty(0)
                .SumAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Models;

namespace YGOProAnalyticsServer.Services.Analyzers.Interfaces
{
    /// <summary>
    /// Analyze server activity.
    /// </summary>
    public interface IServerActivityAnalyzer
    {
        /// <summary>
        /// Numbers the of games from last days.
        /// </summary>
        /// <param name="numberOfDays">The number of days.</param>
        /// <returns>Number of games from last days.</returns>
        Task<int> NumberOfGamesFromLastDaysAsync(int numberOfDays);

        /// <summary>
        /// Numbers the of games from month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <returns>Numbers the of games from month.</returns>
        Task<int> NumberOfGamesFromMonthAsync(int year, int month);

        /// <summary>
        /// Numbers the of games from one day.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Numbers the of games from one day.</returns>
        Task<int> NumberOfGamesFromOneDayAsync(DateTime date);

        /// <summary>
        /// Numbers the of games from one day.
        /// </summary>
        /// <param name="duelLogsFromOneDay">The duel logs from one day.</param>
        /// <returns>Numbers the of games from one day.</returns>
        int NumberOfGamesFromOneDay(IEnumerable<DuelLog> duelLogsFromOneDay);

        /// <summary>
        /// Numbers the of games from one year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>Numbers the of games from one year.</returns>
        Task<int> NumberOfGamesFromOneYearAsync(int year);
    }
}
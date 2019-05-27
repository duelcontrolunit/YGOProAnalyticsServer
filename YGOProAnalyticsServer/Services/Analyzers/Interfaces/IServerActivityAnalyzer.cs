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
        /// Numbers of the games from last days.
        /// </summary>
        /// <param name="numberOfDays">The number of days.</param>
        /// <returns>Numbers of the games from last days.</returns>
        Task<int> NumberOfGamesFromLastDaysAsync(int numberOfDays);

        /// <summary>
        /// Numbers of the games from month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <returns>Numbers of the games from month.</returns>
        Task<int> NumberOfGamesFromMonthAsync(int year, int month);

        /// <summary>
        /// Numbers of the games from one day.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Numbers of the games from one day.</returns>
        Task<int> NumberOfGamesFromOneDayAsync(DateTime date);

        /// <summary>
        /// Numbers of the games from one day.
        /// </summary>
        /// <param name="duelLogsFromOneDay">The duel logs from one day.</param>
        /// <returns>Numbers of the games from one day.</returns>
        int NumberOfGamesFromOneDay(IEnumerable<DuelLog> duelLogsFromOneDay);

        /// <summary>
        /// Numbers of the games from one year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>Numbers of the games from one year.</returns>
        Task<int> NumberOfGamesFromOneYearAsync(int year);
    }
}
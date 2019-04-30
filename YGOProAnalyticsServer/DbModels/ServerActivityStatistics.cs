using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// Statistics per day.
    /// </summary>
    public class ServerActivityStatistics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerActivityStatistics"/> class.
        /// </summary>
        /// <param name="statisticsFromDate">The statistics from date.</param>
        public ServerActivityStatistics(DateTime statisticsFromDate)
        {
            FromDate = statisticsFromDate;
        }

        /// <summary>
        /// The identifier.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Statistics from date.
        /// </summary>
        public DateTime FromDate { get; protected set; }

        /// <summary>
        /// The number of games played that day. <see cref="FromDate"/>
        /// </summary>
        public int NumberOfGames { get; set; } = 0;
    }
}

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
        protected ServerActivityStatistics(int id, DateTime statisticsFromDate, int numberOfGames)
        {
            Id = id;
            FromDate = statisticsFromDate;
            NumberOfGames = numberOfGames;
        }

        public ServerActivityStatistics(DateTime statisticsFromDate)
        {
            FromDate = statisticsFromDate;
        }

        public int Id { get; protected set; }
        public DateTime FromDate { get; protected set; }
        public int NumberOfGames { get; set; } = 0;
    }
}

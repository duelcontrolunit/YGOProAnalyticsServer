using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class BanlistBrowserQueryParams
    {
        /// <summary>
        /// Integer between 1 to inf
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Integer between 1 to inf
        /// </summary>
        public int MinNumberOfGames { get; set; } = 1;

        /// <summary>
        /// Integer between 1 to inf. -1 is special value which mean "use default value";
        /// </summary>
        public int NumberOfResults { get; set; } = -1;

        /// <summary>
        /// Empty string is special value which mean "there is no statistics from date filter"
        /// </summary>
        public string StatisticsFromDate { get; set; } = "";

        /// <summary>
        /// Empty string is special value which mean "there is no statistics from date filter"
        /// </summary>
        public string StatisticsToDate { get; set; } = "";

        /// <summary>
        /// For example TCG, OCG, Traditional or 2019.05 TCG
        /// </summary>
        public string FormatOrName { get; set; } = "";
    }
}

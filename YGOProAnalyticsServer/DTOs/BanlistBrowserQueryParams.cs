using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class BanlistBrowserQueryParams
    {
        public int PageNumber { get; set; } = 1;
        public int MinNumberOfGames { get; set; } = 1;
        public int NumberOfResults { get; set; } = -1;
        public string StatisticsFromDate { get; set; } = "";
        public string StatisticsToDate { get; set; } = "";
    }
}

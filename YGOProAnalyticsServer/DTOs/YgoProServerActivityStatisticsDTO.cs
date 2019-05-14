using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class YgoProServerActivityStatisticsDTO
    {
        public DateTime FromDate { get; set; }

        public int NumberOfGames { get; set; }
    }
}

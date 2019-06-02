using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class DecklistBrowserQueryParametersDTO
    {
        public int PageNumber { get; set; } = 1;
        public int BanlistId { get; set; } = -1;
        public string ArchetypeName { get; set; } = "";
        public int MinNumberOfGames { get; set; } = 10;
        public string StatisticsFromDate { get; set; } = "";
        public string StatisticsToDate { get; set; } = "";
    }
}

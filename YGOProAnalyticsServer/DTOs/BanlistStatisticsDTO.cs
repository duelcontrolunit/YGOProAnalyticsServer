using System;

namespace YGOProAnalyticsServer.DTOs
{
    public class BanlistStatisticsDTO
    {
        public string BanlistName { get; set; }
        public DateTime FromDate { get; set; }
        public int HowManyTimesWasUsed { get; set; }
    }
}
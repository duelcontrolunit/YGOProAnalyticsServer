using System;

namespace YGOProAnalyticsServer.DTOs
{
    public class BanlistStatisticsDTO
    {
        public BanlistStatisticsDTO(
            DateTime fromDate,
            int howManyTimesWasUsed)
        {
            FromDate = fromDate;
            HowManyTimesWasUsed = howManyTimesWasUsed;
        }

        public DateTime FromDate { get; set; }
        public int HowManyTimesWasUsed { get; set; }
    }
}
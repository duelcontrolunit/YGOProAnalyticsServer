using System;

namespace YGOProAnalyticsServer.DTOs
{
    public class DeckStatisticsDTO
    {
        public DateTime DateWhenDeckWasUsed { get; set; }
        public int NumberOfTimesWhenDeckWasUsed { get; set; }
        public int NumberOfTimesWhenDeckWon { get; set; }
    }
}
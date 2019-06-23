using System;

namespace YGOProAnalyticsServer.DTOs
{
    public class ArchetypeStatisticsDTO
    {
        public ArchetypeStatisticsDTO(
            int id,
            DateTime dateWhenArchetypeWasUsed,
            int numberOfDecksWhereWasUsed,
            int numberOfTimesWhenArchetypeWon)
        {
            Id = id;
            DateWhenArchetypeWasUsed = dateWhenArchetypeWasUsed;
            NumberOfDecksWhereWasUsed = numberOfDecksWhereWasUsed;
            NumberOfTimesWhenArchetypeWon = numberOfTimesWhenArchetypeWon;
        }

        public int Id { get; set; }
        public DateTime DateWhenArchetypeWasUsed { get; set; }
        public int NumberOfDecksWhereWasUsed { get; set; }
        public int NumberOfTimesWhenArchetypeWon { get; set; }
    }
}
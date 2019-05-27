using System;
using System.ComponentModel.DataAnnotations;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// Contain informaton about archetype per day.
    /// </summary>
    public class ArchetypeStatistics
    {
        protected ArchetypeStatistics()
        {
        }

        /// <summary>
        /// Initialize archetype statistics. Remember to assign archetype.
        /// </summary>
        /// <param name="dateWhenArchetypeWasUsed">Date of the analysis.</param>
        public ArchetypeStatistics(Archetype archetype, DateTime dateWhenArchetypeWasUsed)
        {
            Archetype = archetype;
            DateWhenArchetypeWasUsed = dateWhenArchetypeWasUsed;
        }

        /// <summary>
        /// Statistic id.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Id of the analyzed archetype.
        /// </summary>
        public int ArchetypeId { get; set; }

        /// <summary>
        /// Analyzed archetype.
        /// </summary>
        [Required]
        public Archetype Archetype { get; set; }

        /// <summary>
        /// Date when archetype was used.
        /// </summary>
        [Required]
        public DateTime DateWhenArchetypeWasUsed { get; protected set; }

        /// <summary>
        /// Contain information how many times archetype was used.
        /// </summary>
        public int NumberOfDecksWhereWasUsed { get; protected set; }

        /// <summary>
        /// Contain information how many times archetype won (counting by decks).
        /// </summary>
        public int NumberOfTimesWhenArchetypeWon { get; protected set; }

        /// <summary>
        /// Add one to NumberOfDecksWhereWasUsed.
        /// </summary>
        public void IncrementNumberOfDecksWhereWasUsed()
        {
            NumberOfDecksWhereWasUsed++;
        }
        /// <summary>
        /// Add amount to NumberOfDecksWhereWasUsed.
        /// If amount is less or equal to 0 the amount is ignored.
        /// </summary>
        public void IncrementNumberOfDecksWhereWasUsedByAmount(int amount)
        {
            if (amount > 0)
            {
                NumberOfDecksWhereWasUsed += amount;
            }
        }

        /// <summary>
        /// Add one to NumberOfTimesWhenArchetypeWin.
        /// </summary>
        public void IncrementNumberOfTimesWhenArchetypeWon()
        {
            NumberOfTimesWhenArchetypeWon++;
        }
        /// <summary>
        /// Add amount to NumberOfTimesWhenArchetypeWin.
        /// If amount is less or equal to 0 the amount is ignored.
        /// </summary>
        public void IncrementNumberOfTimesWhenArchetypeWonByAmount(int amount)
        {
            if (amount > 0)
            {
                NumberOfTimesWhenArchetypeWon += amount;
            }
        }
    }
}

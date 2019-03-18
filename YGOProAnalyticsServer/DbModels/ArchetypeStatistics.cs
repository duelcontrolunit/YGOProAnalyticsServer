using System;
using System.ComponentModel.DataAnnotations;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// Contain informaton about archetype per day.
    /// </summary>
    public class ArchetypeStatistics
    {
        /// <summary>
        /// Initialize archetype statistics. Remember to assign archetype.
        /// </summary>
        /// <param name="dateWhenArchetypeWasUsed">Date of the analysis.</param>
        public ArchetypeStatistics(DateTime dateWhenArchetypeWasUsed)
        {
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
        /// Add one to NumberOfTimesWhenArchetypeWin.
        /// </summary>
        public void IncrementNumberOfTimesWhenArchetypeWon()
        {
            NumberOfTimesWhenArchetypeWon++;
        }
    }
}

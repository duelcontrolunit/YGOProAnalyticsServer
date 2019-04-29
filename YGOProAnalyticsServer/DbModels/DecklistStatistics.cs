using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// Statistics of a decklist
    /// </summary>
    public class DecklistStatistics
    {
        /// <summary>
        /// Decklist Statistics Identifier
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; protected set; }

        /// <summary>
        /// The decklist of which statistics are held.
        /// </summary>
        /// <value>
        /// The decklist.
        /// </value>
        [Required]
        public Decklist Decklist { get; set; }

        /// <summary>
        /// Gets the date when deck was used.
        /// </summary>
        /// <value>
        /// The date when deck was used.
        /// </value>
        [Required]
        public DateTime DateWhenDeckWasUsed { get; protected set; }

        /// <summary>
        /// Gets the number of times when deck was used.
        /// </summary>
        /// <value>
        /// The number of times when deck was used.
        /// </value>
        public int NumberOfTimesWhenDeckWasUsed { get; protected set; }

        /// <summary>
        /// Gets or sets the number of times when deck won.
        /// </summary>
        /// <value>
        /// The number of times when deck won.
        /// </value>
        public int NumberOfTimesWhenDeckWon { get; protected set; }

        /// <summary>
        /// Increments the number of times when deck was used.
        /// </summary>
        public void IncrementNumberOfTimesWhenDeckWasUsed()
        {
            NumberOfTimesWhenDeckWasUsed++;
        }

        /// <summary>
        /// Increments the number of times when deck won.
        /// </summary>
        public void IncrementNumberOfTimesWhenDeckWon()
        {
            NumberOfTimesWhenDeckWon++;
        }
    }
}

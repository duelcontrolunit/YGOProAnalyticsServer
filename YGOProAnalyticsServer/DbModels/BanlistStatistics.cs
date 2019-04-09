using System;
using System.ComponentModel.DataAnnotations;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// Banlist statistics.
    /// </summary>
    public class BanlistStatistics
    {
        protected BanlistStatistics(){ }

        /// <summary>
        /// Create new instance of <see cref="BanlistStatistics"/>.
        /// </summary>
        /// <param name="dateWhenArchetypeWasUsed">Date when archetype was used.</param>
        /// <param name="banlist">Analyzed banlist.</param>
        /// <returns>New instance of <see cref="BanlistStatistics"/>.</returns>
        public static BanlistStatistics Create(DateTime dateWhenArchetypeWasUsed, Banlist banlist)
        {
            return new BanlistStatistics()
            {
                DateWhenArchetypeWasUsed = dateWhenArchetypeWasUsed,
                Banlist = banlist
            };
        }

        /// <summary>
        /// The identifier of the banlist.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Analyzed banlist.
        /// </summary>
        [Required]
        public Banlist Banlist { get; set; }

        /// <summary>
        /// When banlist was used.
        /// </summary>
        [Required]
        public DateTime DateWhenArchetypeWasUsed { get; protected set; }

        /// <summary>
        /// How many times was used.
        /// </summary>
        public int HowManyTimesWasUsed { get; protected set; } = 0;

        /// <summary>
        /// Increment how many times was used.
        /// </summary>
        public void IncrementHowManyTimesWasUsed()
        {
            HowManyTimesWasUsed++;
        }
    }
}

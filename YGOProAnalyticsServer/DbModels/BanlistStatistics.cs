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

        protected static BanlistStatistics Create(DateTime dateWhenArchetypeWasUsed, Banlist banlist)
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
        /// Increment how many times was used
        /// </summary>
        public void IncrementHowManyTimesWasUsed()
        {
            HowManyTimesWasUsed++;
        }
    }
}

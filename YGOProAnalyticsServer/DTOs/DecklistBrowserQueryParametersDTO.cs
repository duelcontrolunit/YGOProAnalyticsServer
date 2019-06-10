using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class DecklistBrowserQueryParametersDTO
    {
        /// <summary>
        /// (1, inf)
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// (1, inf)
        /// </summary>
        public int BanlistId { get; set; } = -1;

        /// <summary>
        /// Name of the archetype.
        /// </summary>
        public string ArchetypeName { get; set; } = "";

        /// <summary>
        /// Find only that decklists whose have at least min number if games.
        /// (1, inf)
        /// </summary>
        public int MinNumberOfGames { get; set; } = 10;

        /// <summary>
        /// Format: yyyy-mm-dd
        /// </summary>
        public string StatisticsFromDate { get; set; } = "";

        /// <summary>
        /// Format: yyyy-mm-dd
        /// </summary>
        public string StatisticsToDate { get; set; } = "";

        /// <summary>
        /// Gets or sets the number of results.
        /// </summary>
        /// <value>
        /// The number of results.
        /// </value>
        public int NumberOfResults { get; set; } = -1;
    }
}

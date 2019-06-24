using System;

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

        /// <summary>
        /// If is set to false, results are sorted by number of wins.
        /// </summary>
        public bool OrderByDescendingByNumberOfGames { get; set; } = false;

        /// <summary>
        /// Example valid input: 12345678;9876544321
        /// </summary>
        public int[] WantedCardsInDeck { get; set; } = Array.Empty<int>();
    }
}

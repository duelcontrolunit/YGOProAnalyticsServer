namespace YGOProAnalyticsServer.DTOs
{
    public class ArchetypeBrowserQueryParams
    {
        /// <summary>
        /// Integer between 1 to inf
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Integer between 1 to inf
        /// </summary>
        public int MinNumberOfGames { get; set; } = 5;

        /// <summary>
        /// Integer between 1 to inf. -1 is special value which mean "use default value";
        /// </summary>
        public int NumberOfResults { get; set; } = -1;

        /// <summary>
        /// Format: yyyy-mm-dd .
        /// Empty string is special value which mean "there is no statistics from date filter"
        /// </summary>
        public string StatisticsFromDate { get; set; } = "";

        /// <summary>
        /// Format: yyyy-mm-dd . 
        /// Empty string is special value which mean "there is no statistics from to filter"
        /// </summary>
        public string StatisticsToDate { get; set; } = "";

        /// <summary>
        /// Archetype name
        /// </summary>
        public string ArchetypeName { get; set; } = "";

        /// <summary>
        /// If is set to false, results are sorted by number of wins.
        /// </summary>
        public bool OrderByDescendingByNumberOfGames { get; set; } = false;
    }
}

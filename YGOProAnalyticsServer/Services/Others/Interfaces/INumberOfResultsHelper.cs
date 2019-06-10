namespace YGOProAnalyticsServer.Services.Others.Interfaces
{
    /// <summary>
    /// Provide support for calculation number of results per page.
    /// </summary>
    public interface INumberOfResultsHelper
    {
        /// <summary>
        /// Gets the number of results per page.
        /// </summary>
        /// <param name="numberOfResultsRequestedByUser">The number of results requested by user.</param>
        int GetNumberOfResultsPerPage(int numberOfResultsRequestedByUser);
    }
}
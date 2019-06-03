using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Validators.Interfaces
{
    /// <summary>
    /// Should provide validation for <see cref="DecklistBrowserQueryParametersDTO"/>.
    /// </summary>
    public interface IDecklistBrowserQueryParametersDtoValidator
    {
        /// <summary>
        /// Returns true if <see cref="DecklistBrowserQueryParametersDTO"/> is valid.
        /// </summary>
        /// <param name="queryParams">The query parameters.</param>
        /// <returns>
        ///   True if <see cref="DecklistBrowserQueryParametersDTO"/> is valid.
        /// </returns>
        bool IsValid(DecklistBrowserQueryParametersDTO queryParams);

        /// <summary>
        /// Returns true if <see cref="DecklistBrowserQueryParametersDTO.ArchetypeName"/> query parameter is valid.
        /// </summary>
        /// <param name="queryParams">The query parameters.</param>
        /// <returns>
        ///   True if <see cref="DecklistBrowserQueryParametersDTO.ArchetypeName"/> query parameter is valid.
        /// </returns>
        bool IsValidArchetypeName(DecklistBrowserQueryParametersDTO queryParams);

        /// <summary>
        /// Returns true if <see cref="DecklistBrowserQueryParametersDTO.BanlistId"/> query parameter is valid.
        /// </summary>
        /// <param name="queryParams">The query parameters.</param>
        /// <returns>
        ///   True if <see cref="DecklistBrowserQueryParametersDTO.BanlistId"/> query parameter is valid.
        /// </returns>
        bool IsValidBanlistId(DecklistBrowserQueryParametersDTO queryParams);

        /// <summary>
        /// Returns true if <see cref="DecklistBrowserQueryParametersDTO.MinNumberOfGames"/> query parameter is valid.
        /// </summary>
        /// <param name="queryParams">The query parameters.</param>
        /// <returns>
        ///   True if <see cref="DecklistBrowserQueryParametersDTO.MinNumberOfGames"/> query parameter is valid.
        /// </returns>
        bool IsValidMinNumberOfGames(DecklistBrowserQueryParametersDTO queryParams);

        /// <summary>
        /// Returns true if <see cref="DecklistBrowserQueryParametersDTO.PageNumber"/> query parameter is valid.
        /// </summary>
        /// <param name="queryParams">The query parameters.</param>
        /// <returns>
        ///   True if <see cref="DecklistBrowserQueryParametersDTO.PageNumber"/> query parameter is valid.
        /// </returns>
        bool IsValidPageNumber(DecklistBrowserQueryParametersDTO queryParams);

        /// <summary>
        /// Returns true if <see cref="DecklistBrowserQueryParametersDTO.StatisticsFromDate"/> query parameter is valid.
        /// </summary>
        /// <param name="queryParams">The query parameters.</param>
        /// <returns>
        ///   True if <see cref="DecklistBrowserQueryParametersDTO.StatisticsFromDate"/> query parameter is valid.
        /// </returns>
        bool IsValidStatisticsFromDate(DecklistBrowserQueryParametersDTO queryParams);

        /// <summary>
        /// Returns true if <see cref="DecklistBrowserQueryParametersDTO.StatisticsToDate"/> query parameter is valid.
        /// </summary>
        /// <param name="queryParams">The query parameters.</param>
        /// <returns>
        ///   True if <see cref="DecklistBrowserQueryParametersDTO.StatisticsToDate"/> query parameter is valid.
        /// </returns>
        bool IsValidStatisticsToDate(DecklistBrowserQueryParametersDTO queryParams);
    }
}
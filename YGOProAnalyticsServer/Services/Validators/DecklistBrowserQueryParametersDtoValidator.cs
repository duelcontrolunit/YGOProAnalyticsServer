using System;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Validators.Interfaces;

namespace YGOProAnalyticsServer.Services.Validators
{
    /// <summary>
    /// Provide possibility to validate <see cref="DecklistBrowserQueryParametersDTO"/> params.
    /// </summary>
    /// <seealso cref="YGOProAnalyticsServer.Services.Validators.Interfaces.IDecklistBrowserQueryParametersDtoValidator" />
    public class DecklistBrowserQueryParametersDtoValidator : IDecklistBrowserQueryParametersDtoValidator
    {
        readonly IDateValidator _dateValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecklistBrowserQueryParametersDtoValidator"/> class.
        /// </summary>
        /// <param name="dateValidator">The date validator.</param>
        /// <exception cref="ArgumentNullException">dateValidator</exception>
        public DecklistBrowserQueryParametersDtoValidator(IDateValidator dateValidator)
        {
            _dateValidator = dateValidator ?? throw new ArgumentNullException(nameof(dateValidator));
        }

        /// <inheritdoc />
        public bool IsValid(DecklistBrowserQueryParametersDTO queryParams)
        {
            return IsValidPageNumber(queryParams)
                   && IsValidBanlistId(queryParams)
                   && IsValidArchetypeName(queryParams)
                   && IsValidMinNumberOfGames(queryParams)
                   && IsValidStatisticsFromDate(queryParams)
                   && IsValidStatisticsToDate(queryParams)
                   && IsValidNumberOfResults(queryParams);
        }

        public bool IsValidNumberOfResults(DecklistBrowserQueryParametersDTO queryParams)
        {
            return queryParams.NumberOfResults > 0 || queryParams.NumberOfResults == -1;
        }

        /// <inheritdoc />
        public bool IsValidPageNumber(DecklistBrowserQueryParametersDTO queryParams)
        {
            return queryParams.PageNumber >= 1;
        }

        /// <inheritdoc />
        public bool IsValidBanlistId(DecklistBrowserQueryParametersDTO queryParams)
        {
            return queryParams.BanlistId >= 1 || queryParams.BanlistId == -1;
        }

        /// <inheritdoc />
        public bool IsValidArchetypeName(DecklistBrowserQueryParametersDTO queryParams)
        {
            return true;
        }

        /// <inheritdoc />
        public bool IsValidMinNumberOfGames(DecklistBrowserQueryParametersDTO queryParams)
        {
            return queryParams.MinNumberOfGames >= 1;
        }

        /// <inheritdoc />
        public bool IsValidStatisticsFromDate(DecklistBrowserQueryParametersDTO queryParams)
        {
            return _dateValidator
                .IsValidFormat(queryParams.StatisticsFromDate, DateFormat.yyyy_MM_dd)
                || queryParams.StatisticsFromDate == "";
        }

        /// <inheritdoc />
        public bool IsValidStatisticsToDate(DecklistBrowserQueryParametersDTO queryParams)
        {
            return _dateValidator
                 .IsValidFormat(queryParams.StatisticsToDate, DateFormat.yyyy_MM_dd)
                 || queryParams.StatisticsToDate == "";
        }
    }
}

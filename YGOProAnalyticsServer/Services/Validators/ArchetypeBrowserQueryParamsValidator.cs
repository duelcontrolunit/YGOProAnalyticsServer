using System;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Validators.Interfaces;

namespace YGOProAnalyticsServer.Services.Validators
{
    /// <summary>
    /// Provide validation for <see cref="ArchetypeBrowserQueryParams"/>
    /// </summary>
    public class ArchetypeBrowserQueryParamsValidator : IArchetypeBrowserQueryParamsValidator
    {
        readonly IDateValidator _dateValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchetypeBrowserQueryParamsValidator"/> class.
        /// </summary>
        /// <param name="dateValidator">The date validator.</param>
        /// <exception cref="ArgumentNullException">dateValidator</exception>
        public ArchetypeBrowserQueryParamsValidator(IDateValidator dateValidator)
        {
            _dateValidator = dateValidator ?? throw new ArgumentNullException(nameof(dateValidator));
        }

        /// <inheritdoc />
        public bool IsValid(ArchetypeBrowserQueryParams queryParams)
        {
            return _isValidPageNumber(queryParams)
                   || _isValidMinNumberOfGames(queryParams)
                   || _isValidNumberOfResults(queryParams)
                   || _isValidStatisticsFromDate(queryParams)
                   || _isValidStatisticsToDate(queryParams)
                   || _isValidArchetypeName(queryParams);
        }

        private bool _isValidArchetypeName(ArchetypeBrowserQueryParams queryParams)
        {
            return true;
        }

        private bool _isValidStatisticsToDate(ArchetypeBrowserQueryParams queryParams)
        {
            return queryParams.StatisticsToDate == ""
                   || _dateValidator.IsValidFormat(queryParams.StatisticsToDate, DateFormat.yyyy_MM_dd);
        }

        private bool _isValidStatisticsFromDate(ArchetypeBrowserQueryParams queryParams)
        {
            return queryParams.StatisticsFromDate == ""
                   || _dateValidator.IsValidFormat(queryParams.StatisticsFromDate, DateFormat.yyyy_MM_dd);
        }

        private bool _isValidNumberOfResults(ArchetypeBrowserQueryParams queryParams)
        {
            return queryParams.NumberOfResults >= 1 || queryParams.NumberOfResults == -1;
        }

        private bool _isValidMinNumberOfGames(ArchetypeBrowserQueryParams queryParams)
        {
            return queryParams.MinNumberOfGames >= 1;
        }

        private bool _isValidPageNumber(ArchetypeBrowserQueryParams queryParams)
        {
            return queryParams.PageNumber >= 1;
        }
    }
}

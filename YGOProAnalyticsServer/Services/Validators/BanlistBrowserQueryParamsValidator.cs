using System;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Validators.Interfaces;

namespace YGOProAnalyticsServer.Services.Validators
{
    public class BanlistBrowserQueryParamsValidator : IBanlistBrowserQueryParamsValidator
    {
        readonly IDateValidator _dateValidator;

        public BanlistBrowserQueryParamsValidator(IDateValidator dateValidator)
        {
            _dateValidator = dateValidator ?? throw new ArgumentNullException(nameof(dateValidator));
        }

        public bool IsValid(BanlistBrowserQueryParams queryParams)
        {
            return _isValidPageNumber(queryParams)
                   && _isValidNumberOfResults(queryParams)
                   && _isValidMinNumberOfGames(queryParams)
                   && _isValidStatisticsDateFrom(queryParams)
                   && _isValisStatisticsToDate(queryParams);
        }

        private bool _isValisStatisticsToDate(BanlistBrowserQueryParams queryParams)
        {
            return queryParams.StatisticsFromDate == ""
                   || _dateValidator.IsValidFormat(queryParams.StatisticsToDate, DateFormat.yyyy_MM_dd);
        }

        private bool _isValidStatisticsDateFrom(BanlistBrowserQueryParams queryParams)
        {
            return queryParams.StatisticsFromDate == ""
                   || _dateValidator.IsValidFormat(queryParams.StatisticsFromDate, DateFormat.yyyy_MM_dd);
        }

        private bool _isValidPageNumber(BanlistBrowserQueryParams queryParams)
        {
            return queryParams.PageNumber > 0;
        }

        private bool _isValidNumberOfResults(BanlistBrowserQueryParams queryParams)
        {
            return queryParams.NumberOfResults > 0 || queryParams.NumberOfResults == -1;
        }

        private bool _isValidMinNumberOfGames(BanlistBrowserQueryParams queryParams)
        {
            return queryParams.MinNumberOfGames > 0;
        }
    }
}

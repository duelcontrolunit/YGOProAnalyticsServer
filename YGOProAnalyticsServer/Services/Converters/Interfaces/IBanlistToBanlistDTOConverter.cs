using System.Collections.Generic;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using System;

namespace YGOProAnalyticsServer.Services.Converters.Interfaces
{
    /// <summary>
    /// Provide convert from banlist to dto feature.
    /// </summary>
    public interface IBanlistToBanlistDTOConverter
    {
        /// <summary>
        /// Converts the specified banlist to requested type.
        /// </summary>
        /// <param name="banlistToConvert">The banlist to convert.</param>
        /// <param name="statisticsFrom">The statistics from.</param>
        /// <param name="statisticsTo">The statistics to.</param>
        IEnumerable<BanlistWithHowManyWasUsed> Convert(
            IEnumerable<Banlist> banlistToConvert,
            DateTime? statisticsFrom = null,
            DateTime? statisticsTo = null);
    }
}
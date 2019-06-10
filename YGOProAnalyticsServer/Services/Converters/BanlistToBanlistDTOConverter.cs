using System.Linq;
using System.Collections.Generic;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using System;

namespace YGOProAnalyticsServer.Services.Converters
{
    /// <summary>
    /// Provide convert from banlist to dto feature.
    /// </summary>
    public class BanlistToBanlistDTOConverter : IBanlistToBanlistDTOConverter
    {
        /// <inheritdoc />
        public IEnumerable<BanlistWithHowManyWasUsed> Convert(
            IEnumerable<Banlist> banlistToConvert,
            DateTime? statisticsFrom = null,
            DateTime? statisticsTo = null)
        {
            var dtos = new List<BanlistWithHowManyWasUsed>(banlistToConvert.Count());
            foreach (var banlist in banlistToConvert)
            {
                var dto = new BanlistWithHowManyWasUsed(
                        id: banlist.Id,
                        name: banlist.Name,
                        format: banlist.Format,
                        releaseDate: banlist.ReleaseDate,
                        howManyTimeWasUsed: _howManyTimesWasUsedInRange(banlist, statisticsFrom, statisticsTo)
                    );
                dtos.Add(dto);
            }

            return dtos;
        }

        private int _howManyTimesWasUsedInRange(Banlist banlist, DateTime? statisticsFrom, DateTime? statisticsTo)
        {
            if (statisticsFrom != null && statisticsTo == null)
            {
                return banlist
                    .Statistics
                    .Where(x => x.DateWhenBanlistWasUsed >= statisticsFrom)
                    .Sum(x => x.HowManyTimesWasUsed);
            }
            else
           if (statisticsFrom == null && statisticsTo != null)
            {
                return banlist
                    .Statistics
                    .Where(x => x.DateWhenBanlistWasUsed<= statisticsTo)
                    .Sum(x => x.HowManyTimesWasUsed);
            }
            else if (statisticsFrom != null && statisticsTo != null)
            {
                return banlist
                    .Statistics
                    .Where(x => x.DateWhenBanlistWasUsed >= statisticsFrom && x.DateWhenBanlistWasUsed <= statisticsTo)
                    .Sum(x => x.HowManyTimesWasUsed);
            }
            else
            {
                return banlist
                    .Statistics
                    .Sum(x => x.HowManyTimesWasUsed);
            }
        }
    }
}

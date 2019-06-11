using System.Linq;
using System.Collections.Generic;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using System;
using YGOProAnalyticsServer.Services.Factories.Interfaces;

namespace YGOProAnalyticsServer.Services.Converters
{

    /// <summary>
    /// Provide convert from banlist to dto feature.
    /// </summary>
    public class BanlistToBanlistDTOConverter : IBanlistToBanlistDTOConverter
    {
        readonly IDecksDtosFactory _decksDtosFactory;

        public BanlistToBanlistDTOConverter(IDecksDtosFactory decksDtosFactory)
        {
            _decksDtosFactory = decksDtosFactory ?? throw new ArgumentNullException(nameof(decksDtosFactory));
        }

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

        /// <inheritdoc />
        public BanlistWithStatisticsDTO Convert(Banlist banlistToConvert)
        {
            return new BanlistWithStatisticsDTO(
                    name: banlistToConvert.Name,
                    format: banlistToConvert.Format,
                    releaseDate: banlistToConvert.ReleaseDate,
                    bannedCards: _decksDtosFactory.CreateDeckDto(banlistToConvert.ForbiddenCards),
                    limitedCards: _decksDtosFactory.CreateDeckDto(banlistToConvert.LimitedCards),
                    semiLimitedCards: _decksDtosFactory.CreateDeckDto(banlistToConvert.SemiLimitedCards),
                    statistics: _getBanlistStatisticsDtos(banlistToConvert)
                );
        }

        private List<BanlistStatisticsDTO> _getBanlistStatisticsDtos(Banlist banlistToConvert)
        {
            var statistics = new List<BanlistStatisticsDTO>();
            foreach (var statistic in banlistToConvert.Statistics)
            {
                statistics.Add(new BanlistStatisticsDTO(
                    statistic.DateWhenBanlistWasUsed,
                    statistic.HowManyTimesWasUsed));
            }

            return statistics;
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

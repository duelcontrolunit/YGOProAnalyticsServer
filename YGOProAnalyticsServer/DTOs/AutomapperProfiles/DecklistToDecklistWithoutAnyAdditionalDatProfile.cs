using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs.AutomapperProfiles.Converters;

namespace YGOProAnalyticsServer.DTOs.AutomapperProfiles
{
    public class DecklistToDecklistWithoutAnyAdditionalDatProfile : Profile
    {
        readonly DecklistStatisticsToTotalNumberOfGames _statisticsToTotalNumberOfGamesConverter
            = new DecklistStatisticsToTotalNumberOfGames();

        readonly DecklistStatisticsToTotalNumberOfWinsConverter _statisticsToTotalNumberOfWinsConverter
            = new DecklistStatisticsToTotalNumberOfWinsConverter();

        public DecklistToDecklistWithoutAnyAdditionalDatProfile()
        {
            CreateMap<Decklist, DecklistWithNumberOfGamesAndWinsDTO>()
                .ConstructUsing(x => new DecklistWithNumberOfGamesAndWinsDTO(
                                x.Id,
                                x.Name,
                                x.WhenDecklistWasFirstPlayed,
                                0,
                                0)
                 )
                 .ForMember(
                    x => x.NumberOfWins,
                    y => y.ConvertUsing(_statisticsToTotalNumberOfWinsConverter, z => z.DecklistStatistics)
                 )
                .ForMember(
                    x => x.NumberOfGames,
                    y => y.ConvertUsing(_statisticsToTotalNumberOfGamesConverter, z => z.DecklistStatistics)
                 );
        }
    }
}

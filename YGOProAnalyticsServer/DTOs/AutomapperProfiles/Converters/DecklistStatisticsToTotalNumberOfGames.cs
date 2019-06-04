using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.DTOs.AutomapperProfiles.Converters
{
    public class DecklistStatisticsToTotalNumberOfGames
        : IValueConverter<ICollection<DecklistStatistics>, int>
    {
        public int Convert(ICollection<DecklistStatistics> sourceMember, ResolutionContext context)
        {
            return sourceMember.Sum(x => x.NumberOfTimesWhenDeckWasUsed);
        }
    }
}

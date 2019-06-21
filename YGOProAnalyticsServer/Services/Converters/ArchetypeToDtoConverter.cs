using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Converters.Interfaces;

namespace YGOProAnalyticsServer.Services.Converters
{
    /// <summary>
    /// Convert <see cref="Archetype"/> to DTO.
    /// </summary>
    public class ArchetypeToDtoConverter : IArchetypeToDtoConverter
    {
        /// <inheritdoc />
        public IEnumerable<ArchetypeWithHowManyWinsAndHowManyWasUsed> Convert(
            IEnumerable<Archetype> archetypes,
            DateTime? statisticsFrom = null,
            DateTime? statisticsTo = null)
        {
            var dtos = new List<ArchetypeWithHowManyWinsAndHowManyWasUsed>(archetypes.Count());
            foreach (var archetype in archetypes)
            {
                var dto = new ArchetypeWithHowManyWinsAndHowManyWasUsed(
                        id: archetype.Id,
                        name: archetype.Name,
                        howManyWasUsed: _howManyWasUsedInDateTimeRange(statisticsFrom, statisticsTo, archetype),
                        howManyWon: _howManyTimesWonInDateTimeRange(statisticsFrom, statisticsTo, archetype)
                    );
                dtos.Add(dto);
            }

            return dtos;
        }

        private int _howManyTimesWonInDateTimeRange(
            DateTime? statisticsFrom,
            DateTime? statisticsTo,
            Archetype archetype)
        {
           if (statisticsFrom != null && statisticsTo == null)
           {
                return archetype
                    .Statistics
                    .Where(x => x.DateWhenArchetypeWasUsed >= statisticsFrom)
                    .Sum(x => x.NumberOfTimesWhenArchetypeWon);
           }
           else
           if (statisticsFrom == null && statisticsTo != null)
           {
                return archetype
                    .Statistics
                    .Where(x => x.DateWhenArchetypeWasUsed <= statisticsTo)
                    .Sum(x => x.NumberOfTimesWhenArchetypeWon);
           }
           else if (statisticsFrom != null && statisticsTo != null)
           {
                return archetype
                    .Statistics
                    .Where(x => x.DateWhenArchetypeWasUsed >= statisticsFrom 
                             && x.DateWhenArchetypeWasUsed <= statisticsTo)
                    .Sum(x => x.NumberOfTimesWhenArchetypeWon);
           }
           else
           {
                return archetype
                    .Statistics
                    .Sum(x => x.NumberOfTimesWhenArchetypeWon);
           }
        }

        private int _howManyWasUsedInDateTimeRange(DateTime? statisticsFrom, DateTime? statisticsTo, Archetype archetype)
        {
           if (statisticsFrom != null && statisticsTo == null)
           {
                return archetype
                    .Statistics
                    .Where(x => x.DateWhenArchetypeWasUsed >= statisticsFrom)
                    .Sum(x => x.NumberOfDecksWhereWasUsed);
           }
           else
           if (statisticsFrom == null && statisticsTo != null)
           {
                return archetype
                    .Statistics
                    .Where(x => x.DateWhenArchetypeWasUsed <= statisticsTo)
                    .Sum(x => x.NumberOfDecksWhereWasUsed);
           }
           else if (statisticsFrom != null && statisticsTo != null)
           {
                return archetype
                    .Statistics
                    .Where(x => x.DateWhenArchetypeWasUsed >= statisticsFrom
                             && x.DateWhenArchetypeWasUsed <= statisticsTo)
                    .Sum(x => x.NumberOfDecksWhereWasUsed);
           }
           else
           {
                return archetype
                    .Statistics
                    .Sum(x => x.NumberOfDecksWhereWasUsed);
           }
        }
    }
}

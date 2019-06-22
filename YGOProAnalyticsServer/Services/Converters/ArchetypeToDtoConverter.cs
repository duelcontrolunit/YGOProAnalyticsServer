using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Services.Factories.Interfaces;

namespace YGOProAnalyticsServer.Services.Converters
{
    /// <summary>
    /// Provide archetype to dto conversion.
    /// </summary>
    public class ArchetypeToDtoConverter : IArchetypeToDtoConverter
    {
        IDecksDtosFactory _decksFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchetypeToDtoConverter"/> class.
        /// </summary>
        /// <param name="decksFactory">The decks factory.</param>
        /// <exception cref="ArgumentNullException">decksFactory</exception>
        public ArchetypeToDtoConverter(IDecksDtosFactory decksFactory)
        {
            _decksFactory = decksFactory ?? throw new ArgumentNullException(nameof(decksFactory));
        }

        /// <inheritdoc />
        public ConcreteArchetypeDTO Convert(Archetype archetype)
        {
            return new ConcreteArchetypeDTO(
                    id: archetype.Id,
                    name: archetype.Name,
                    isPureArchetype: archetype.IsPureArchetype,
                    cardsInArchetype: _decksFactory.CreateDeckDto(archetype.Cards),
                    statistics: Convert(archetype.Statistics)
                );
        }

        public IEnumerable<ArchetypeStatisticsDTO> Convert(IEnumerable<ArchetypeStatistics> statistics)
        {
            var dtos = new List<ArchetypeStatisticsDTO>(statistics.Count());
            foreach (var statistic in statistics)
            {
                var dto = new ArchetypeStatisticsDTO(
                    statistic.Id,
                    statistic.DateWhenArchetypeWasUsed,
                    statistic.NumberOfDecksWhereWasUsed,
                    statistic.NumberOfTimesWhenArchetypeWon);
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}

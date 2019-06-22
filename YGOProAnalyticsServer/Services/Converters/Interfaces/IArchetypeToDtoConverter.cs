using System;
using System.Collections;
using System.Collections.Generic;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Converters.Interfaces
{
    /// <summary>
    /// Convert <see cref="Archetype"/> to DTO.
    /// </summary>
    public interface IArchetypeToDtoConverter
    {
        /// <summary>
        /// Converts <see cref="Archetype"/> to <see cref="ConcreteArchetypeDTO"/>.
        /// </summary>
        /// <param name="archetype">The archetype.</param>
        ConcreteArchetypeDTO Convert(Archetype archetype);

        /// <summary>
        /// Converts the specified statistics.
        /// </summary>
        /// <param name="statistics">The statistics.</param>
        IEnumerable<ArchetypeStatisticsDTO> Convert(IEnumerable<ArchetypeStatistics> statistics);
        /// Converts <see cref="Archetype"/> collection to <see cref="ArchetypeWithHowManyWinsAndHowManyWasUsed"/> collection.
        /// </summary>
        /// <param name="archetype">The archetype.</param>
        /// <param name="statisticsFrom">The statistics from.</param>
        /// <param name="statisticsTo">The statistics to.</param>
        IEnumerable<ArchetypeWithHowManyWinsAndHowManyWasUsed> Convert(
            IEnumerable<Archetype> archetype,
            DateTime? statisticsFrom = null,
            DateTime? statisticsTo = null);
    }
}
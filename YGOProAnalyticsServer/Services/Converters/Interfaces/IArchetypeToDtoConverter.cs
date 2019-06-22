using System.Collections.Generic;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Converters.Interfaces
{
    /// <summary>
    /// Provide archetype to dto conversion.
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
    }
}
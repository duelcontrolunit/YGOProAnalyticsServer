using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using System.Collections.Generic;

namespace YGOProAnalyticsServer.Services.Converters.Interfaces
{
    /// <summary>
    /// Provide convert decklist to dto decklist feature.
    /// </summary>
    public interface IDecklistToDecklistDtoConverter
    {
        /// <summary>
        /// Convert <see cref="Decklist"/> to <see cref="DecklistWithStatisticsDTO"/>
        /// </summary>
        /// <param name="decklist">The decklist.</param>
        /// <returns>New DecklistWithStatisticsDTO.</returns>
        DecklistWithStatisticsDTO Convert(Decklist decklist);

        /// <summary>
        /// Convert <see cref="Decklist"/> collection to  <see cref="DecklistWithStatisticsDTO"/> collection.
        /// </summary>
        /// <param name="decklists">The decklists.</param>
        IEnumerable<DecklistWithStatisticsDTO> Convert(IEnumerable<Decklist> decklists);
        IEnumerable<DecklistWithoutAnyAdditionalDataDTO> Convert(IEnumerable<Decklist> decklists, System.DateTime? statisticsFrom = null, System.DateTime? statisticsTo = null);
    }
}
using System;
using System.Collections.Generic;

namespace YGOProAnalyticsServer.DTOs
{
    public class DecklistBrowserResultsDTO
    {
        public DecklistBrowserResultsDTO(
            int totalNumberOfPages,
            IEnumerable<DecklistWithNumberOfGamesAndWinsDTO> decklistWithNumberOfGamesAndWins)
        {
            TotalNumberOfPages = totalNumberOfPages;
            DecklistWithNumberOfGamesAndWins = decklistWithNumberOfGamesAndWins 
                ?? throw new ArgumentNullException(nameof(decklistWithNumberOfGamesAndWins));
        }

        public int TotalNumberOfPages { get; set; }
        public IEnumerable<DecklistWithNumberOfGamesAndWinsDTO> DecklistWithNumberOfGamesAndWins { get; set; }
    }
}

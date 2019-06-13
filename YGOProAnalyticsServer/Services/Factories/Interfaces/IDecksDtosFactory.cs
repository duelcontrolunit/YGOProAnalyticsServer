using System.Collections.Generic;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Factories.Interfaces
{
    /// <summary>
    /// Provide easy to use API for creating different decks DTOs.
    /// </summary>
    public interface IDecksDtosFactory
    {
        /// <summary>
        /// Creates the <see cref="MainDeckDTO"/>.
        /// </summary>
        /// <param name="decklist">The decklist.</param>
        /// <returns><see cref="MainDeckDTO"/></returns>
        MainDeckDTO CreateMainDeckDto(Decklist decklist);

        /// <summary>
        /// Creates the <see cref="ExtraDeckDTO"/>.
        /// </summary>
        /// <param name="decklist">The decklist.</param>
        /// <returns><see cref="ExtraDeckDTO"/></returns>
        ExtraDeckDTO CreateExtraDeckDto(Decklist decklist);

        /// <summary>
        /// Creates the <see cref="DeckDTO"/>.
        /// </summary>
        /// <param name="decklist">The decklist.</param>
        /// <returns><see cref="DeckDTO"/></returns>
        DeckDTO CreateDeckDto(Decklist decklist);

        /// <summary>
        /// Creates the <see cref="DeckDTO"/>.
        /// </summary>
        /// <param name="decklist">The decklist.</param>
        /// <returns><see cref="DeckDTO"/></returns>
        DeckDTO CreateDeckDto(IEnumerable<Card> cards);
    }
}
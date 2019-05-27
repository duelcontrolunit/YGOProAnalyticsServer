using System.Collections.Generic;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Models;

namespace YGOProAnalyticsServer.Services.Analyzers.Interfaces
{
    /// <summary>
    /// Used to analyze the decklist for the archetype in it.
    /// </summary>
    public interface IArchetypeAndDecklistAnalyzer
    {
        NumberOfDuplicatesWithListOfDecklists RemoveDuplicateDecklistsFromListOfDecklists(Decklist decklist, IEnumerable<Decklist> listOfDecks);

        /// <summary>Sets the decklist archetype from archetype cards used in it.</summary>
        /// <param name="decklist">The decklist (Must contain non empty list of cards in deck).</param>
        /// <param name="dateWhenDecklistWasUsed">Date of when the decklist passed was used.</param>
        /// <returns>Decklist with archetype set</returns>
        Archetype GetArchetypeOfTheDecklistWithStatistics(Decklist decklist, System.DateTime dateWhenDecklistWasUsed);

        /// <summary>
        /// Checks if decklists are duplicate.
        /// </summary>
        /// <param name="decklist1">The decklist1.</param>
        /// <param name="decklist2">The decklist2.</param>
        /// <returns></returns>
        bool CheckIfDecklistsAreDuplicate(Decklist decklist1, Decklist decklist2);
    }
}
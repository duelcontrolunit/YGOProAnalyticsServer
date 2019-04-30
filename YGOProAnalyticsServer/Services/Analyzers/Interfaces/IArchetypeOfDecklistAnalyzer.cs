using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Analyzers.Interfaces
{
    /// <summary>
    /// Used to analyze the decklist for the archetype in it.
    /// </summary>
    public interface IArchetypeOfDecklistAnalyzer
    {
        /// <summary>
        /// Sets the decklist archetype from archetype cards used in it.
        /// </summary>
        /// <param name="decklist">The decklist (Must contain non empty list of cards in deck).</param>
        /// <returns>Decklist with archetype set</returns>
        Task<Decklist> SetDecklistArchetypeFromArchetypeCardsUsedInIt(Decklist decklist);
    }
}
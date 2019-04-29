using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Analyzers
{

    /// <summary>Used to analyze the decklist for the archetype in it.</summary>
    public class ArchetypeOfDecklistAnalyzer
    {
        private readonly YgoProAnalyticsDatabase _db;

        /// <summary>Initializes a new instance of the <see cref="ArchetypeOfDecklistAnalyzer"/> class.</summary>
        /// <param name="db">The database.</param>
        public ArchetypeOfDecklistAnalyzer(YgoProAnalyticsDatabase db)
        {
            _db = db;
        }

        /// <summary>Sets the decklist archetype from archetypical cards used in it.</summary>
        /// <param name="decklist">The decklist.</param>
        /// <returns>Decklist with Archetype set</returns>
        public Decklist SetDecklistArchetypeFromArchetypeCardsUsedInIt(Decklist decklist)
        {
            decklist.Archetype = _getNameOfMostUsedArchetypeInDecklist(decklist);
            return decklist;
        }

        private Archetype _getNameOfMostUsedArchetypeInDecklist(Decklist decklist)
        {
            List<Card> fullDeck = decklist.MainDeck.Concat(decklist.ExtraDeck).Concat(decklist.SideDeck).ToList();
            List<Archetype> archetypes = fullDeck.ConvertAll<Archetype>(x => x.Archetype);
            archetypes.RemoveAll(x => x.Name == Archetype.Default);
            List<Archetype> uniqueArchetypesInDeck = archetypes.GroupBy(x => x.Name).Select(x => x.First()).ToList();
            var amountOfMostUsedArchetypeCardsInDeck = 0;
            Archetype nameOfTheMostUsedArchetype = null;
            foreach (var ar in uniqueArchetypesInDeck)
            {
                int archetypeUsedCount = archetypes.Where(x => x.Name == ar.Name).Count();
                if (archetypeUsedCount > amountOfMostUsedArchetypeCardsInDeck)
                {
                    amountOfMostUsedArchetypeCardsInDeck = archetypes.Where(x => x.Name == ar.Name).Count();
                    nameOfTheMostUsedArchetype = ar;
                }
            }
            return nameOfTheMostUsedArchetype;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Analyzers.Interfaces;

namespace YGOProAnalyticsServer.Models
{
    /// <summary>
    ///Class should be returned by 
    /// <see cref="IArchetypeAndDecklistAnalyzer.RemoveDuplicateDecklistsFromListOfDecklists(Decklist, List{Decklist})"/>
    ///</summary>
    public class NumberOfDuplicatesWithListOfDecklists
    {
        public NumberOfDuplicatesWithListOfDecklists(
            int duplicateCount, 
            List<Decklist> newListOfDecklists, 
            Decklist decklistThatWasChecked)
        {
            DuplicateCount = duplicateCount;
            NewListOfDecklists = newListOfDecklists ?? throw new ArgumentNullException(nameof(newListOfDecklists));
            DecklistThatWasChecked = decklistThatWasChecked ?? throw new ArgumentNullException(nameof(decklistThatWasChecked));
        }

        /// <summary>
        /// Gets the duplicate count.
        /// </summary>
        /// <value>
        /// The duplicate count.
        /// </value>
        public int DuplicateCount { get; }

        /// <summary>
        /// List of Decklists without duplicates of a decklist given in the argument of 
        /// <see cref="IArchetypeAndDecklistAnalyzer.RemoveDuplicateDecklistsFromListOfDecklists(Decklist, List{Decklist})"/>
        /// </summary>
        /// <value>
        /// The new list of decklists.
        /// </value>
        public List<Decklist> NewListOfDecklists { get; }

        /// <summary>
        /// The decklist that was checked.
        /// </summary>
        /// <value>
        /// The decklist that was checked.
        /// </value>
        public Decklist DecklistThatWasChecked { get; }
    }
}

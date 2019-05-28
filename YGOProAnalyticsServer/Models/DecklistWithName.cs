using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Models
{
    /// <summary>
    /// Representation of Decklist taken from zip file.
    /// </summary>
    public class DecklistWithName
    {
        /// <summary>Initializes a new instance of the <see cref="DecklistWithName"/> class.</summary>
        /// <param name="decklistFileName">Name of the decklist.</param>
        /// <param name="decklistData">The decklist data.</param>
        public DecklistWithName(string decklistFileName, string decklistData)
        {
            DecklistFileName = decklistFileName;
            DecklistData = decklistData;
        }

        /// <summary>Gets or sets the filename of the decklist.</summary>
        /// <value>The filename of the decklist.</value>
        public string DecklistFileName { get; protected set; }
        /// <summary>Gets or sets the decklist data.</summary>
        /// <value>The decklist data.</value>
        public string DecklistData { get; protected set; }
    }
}

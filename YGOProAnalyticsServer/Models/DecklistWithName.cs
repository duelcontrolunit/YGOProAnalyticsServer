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
        /// <param name="decklistName">Name of the decklist.</param>
        /// <param name="decklistData">The decklist data.</param>
        public DecklistWithName(string decklistName, string decklistData)
        {
            DecklistName = decklistName;
            DecklistData = decklistData;
        }

        /// <summary>Gets or sets the name of the decklist.</summary>
        /// <value>The name of the decklist.</value>
        public string DecklistName { get; protected set; }
        /// <summary>Gets or sets the decklist data.</summary>
        /// <value>The decklist data.</value>
        public string DecklistData { get; protected set; }
    }
}

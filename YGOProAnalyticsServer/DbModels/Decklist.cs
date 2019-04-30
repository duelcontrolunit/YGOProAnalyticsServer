using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>Representation of a Decklist</summary>
    public class Decklist
    {

        protected Decklist(int id, string name, DateTime whenDecklistWasFirstPlayed)
        {
            Id = id;
            Name = name;
            WhenDecklistWasFirstPlayed = whenDecklistWasFirstPlayed;
        }
        /// <summary>Initializes a new instance of the <see cref="Decklist"/> class.</summary>
        /// <param name="name">The name of the decklist.</param>
        /// <param name="archetype">The archetype of the decklist.</param>
        /// <param name="whenDecklistWasFirstPlayed">DateTime when decklist was first played.</param>
        public Decklist(string name, Archetype archetype, DateTime whenDecklistWasFirstPlayed)
        {
            Name = name;
            Archetype = archetype;
            WhenDecklistWasFirstPlayed = whenDecklistWasFirstPlayed;
        }

        public Decklist(ICollection<Card> mainDeck, ICollection<Card> extraDeck, ICollection<Card> sideDeck)
        {
            MainDeck = mainDeck;
            ExtraDeck = extraDeck;
            SideDeck = sideDeck;
        }

        /// <summary>
        /// Decklist identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; protected set; }

        /// <summary>
        /// The name of the Decklist.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The main deck.
        /// </summary>
        /// <value>
        /// The main deck.
        /// </value>
        public ICollection<Card> MainDeck { get; protected set; } = new List<Card>();

        /// <summary>
        /// The extra deck.
        /// </summary>
        /// <value>
        /// The extra deck.
        /// </value>
        public ICollection<Card> ExtraDeck { get; protected set; } = new List<Card>();

        /// <summary>
        /// The side deck.
        /// </summary>
        /// <value>
        /// The side deck.
        /// </value>
        public ICollection<Card> SideDeck { get; protected set; } = new List<Card>();

        /// <summary>
        /// The archetype of the Decklist.
        /// </summary>
        /// <value>
        /// The archetype of the Decklist.
        /// </value>
        [Required]
        public Archetype Archetype { get; set; }

        /// <summary>
        /// DateTime when decklist was first played.
        /// </summary>
        /// <value>
        /// DateTime when decklist was first played.
        /// </value>
        [Required]
        public DateTime WhenDecklistWasFirstPlayed { get; set; }

        /// <summary>
        /// Statistics of the decklist.
        /// </summary>
        /// <value>
        /// The decklist statistics.
        /// </value>
        public ICollection<DecklistStatistics> DecklistStatistics { get; protected set; } = new List<DecklistStatistics>();
    }
}

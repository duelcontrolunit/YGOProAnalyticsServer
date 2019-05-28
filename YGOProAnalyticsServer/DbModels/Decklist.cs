using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database.ManyToManySupport;
using YGOProAnalyticsServer.DbModels.DbJoinModels;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>Representation of a Decklist</summary>
    public class Decklist
    {
        public static readonly string IncludeMainDeckCards = $"{nameof(CardsInMainDeckJoin)}.{nameof(CardInMainDeckDecklistJoin.Card)}";
        public static readonly string IncludeExtraDeckCards = $"{nameof(CardsInExtraDeckJoin)}.{nameof(CardInExtraDeckDecklistJoin.Card)}";
        public static readonly string IncludeSideDeckCards = $"{nameof(CardsInSideDeckJoin)}.{nameof(CardInSideDeckDecklistJoin.Card)}";

        protected Decklist()
        {
            MainDeck = new JoinCollectionFacade<Card, Decklist, CardInMainDeckDecklistJoin>(this, CardsInMainDeckJoin);
            ExtraDeck = new JoinCollectionFacade<Card, Decklist, CardInExtraDeckDecklistJoin>(this, CardsInExtraDeckJoin);
            SideDeck = new JoinCollectionFacade<Card, Decklist, CardInSideDeckDecklistJoin>(this, CardsInSideDeckJoin);
        }

        public Decklist(IList<Card> mainDeck, IList<Card> extraDeck, IList<Card> sideDeck)
        {
            MainDeck = new JoinCollectionFacade<Card, Decklist, CardInMainDeckDecklistJoin>(this, CardsInMainDeckJoin);
            ExtraDeck = new JoinCollectionFacade<Card, Decklist, CardInExtraDeckDecklistJoin>(this, CardsInExtraDeckJoin);
            SideDeck = new JoinCollectionFacade<Card, Decklist, CardInSideDeckDecklistJoin>(this, CardsInSideDeckJoin);

            foreach (var card in mainDeck)
            {
                MainDeck.Add(card);
            }

            foreach (var card in extraDeck)
            {
                ExtraDeck.Add(card);
            }

            foreach (var card in sideDeck)
            {
                SideDeck.Add(card);
            }
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
        [NotMapped]
        public ICollection<Card> MainDeck { get; protected set; } = new List<Card>();
        public ICollection<CardInMainDeckDecklistJoin> CardsInMainDeckJoin { get; set; } = new List<CardInMainDeckDecklistJoin>();

        /// <summary>
        /// The extra deck.
        /// </summary>
        /// <value>
        /// The extra deck.
        /// </value>
        [NotMapped]
        public ICollection<Card> ExtraDeck { get; protected set; } = new List<Card>();
        public ICollection<CardInExtraDeckDecklistJoin> CardsInExtraDeckJoin { get; set; } = new List<CardInExtraDeckDecklistJoin>();

        /// <summary>
        /// The side deck.
        /// </summary>
        /// <value>
        /// The side deck.
        [NotMapped]
        public ICollection<Card> SideDeck { get; protected set; } = new List<Card>();
        public ICollection<CardInSideDeckDecklistJoin> CardsInSideDeckJoin { get; set; } = new List<CardInSideDeckDecklistJoin>();

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

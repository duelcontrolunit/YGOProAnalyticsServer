using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// Forbidden/limited list of cards
    /// </summary>
    public class Banlist
    {
        /// <summary>
        /// Id of the banlist
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Name of the banlist
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// List of forbidden cards
        /// </summary>
        public ICollection<Card> ForbiddenCards { get; protected set; }

        /// <summary>
        /// List of limited cards
        /// </summary>
        public ICollection<Card> LimitedCards { get; protected set; }

        /// <summary>
        /// List of semi-limited cards
        /// </summary>
        public ICollection<Card> SemiLimitedCards { get; protected set; }

        /// <summary>
        /// For example TCG, OCG
        /// </summary>
        [NotMapped]
        public string Format
        {
            get
            {
                return Name.Substring(Name.IndexOf(' ') + 1);
            }
            protected set { }
        }

        /// <summary>
        /// All cards on the banlist
        /// </summary>
        [NotMapped]
        public IEnumerable<Card> AllCards
        {
            get
            {
                var allCards = new List<Card>();
                var forbiddenCards = (IEnumerable<Card>)ForbiddenCards;
                var limitedCards = (IEnumerable<Card>)LimitedCards;
                var semiLimitedCards = (IEnumerable<Card>)SemiLimitedCards;
                allCards.AddRange(forbiddenCards);
                allCards.AddRange(limitedCards);
                allCards.AddRange(semiLimitedCards);

                return allCards;
            }
            protected set { }
        }

        /// <summary>
        /// Release date of the banlist
        /// </summary>
        [NotMapped]
        public DateTime ReleaseDate
        {
            get
            {
                return DateTime.ParseExact(Name.Substring(0, Name.IndexOf(' ')), "YYYY.MM", CultureInfo.InvariantCulture);
            }
            protected set { }
        }
    }
}

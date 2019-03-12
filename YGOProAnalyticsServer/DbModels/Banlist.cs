using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database.ManyToManySupport;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// Forbidden/limited list of cards
    /// </summary>
    public class Banlist
    {
        /// <summary>
        /// Include string required to include forbidden cards.
        /// </summary>
        public static readonly string IncludeWithForbiddenCards = $"{nameof(ForbiddenCardsJoin)}.{nameof(ForbiddenCardBanlistJoin.Card)}";

        /// <summary>
        /// Include string required to include limited cards.
        /// </summary>
        public static readonly string IncludeWithLimitedCards = $"{nameof(LimitedCardsJoin)}.{nameof(LimitedCardBanlistJoin.Card)}";

        /// <summary>
        /// Include string required to include forbidden cards.
        /// </summary>
        public static readonly string IncludeWithSemiLimitedCards = $"{nameof(SemiLimitedCardsJoin)}.{nameof(SemiLimitedCardBanlistJoin.Card)}";

        /// <summary>
        /// Initialize banlist.
        /// </summary>
        /// <param name="name">Valid name should look like: "YYYY.MM Format" for example "2010 TCG" </param>
        protected Banlist(int id, string name)
        {
            Id = id;
            Name = name;
            ForbiddenCards = new JoinCollectionFacade<Card, Banlist, ForbiddenCardBanlistJoin>(this, ForbiddenCardsJoin);
            LimitedCards = new JoinCollectionFacade<Card, Banlist, LimitedCardBanlistJoin>(this, LimitedCardsJoin);
            SemiLimitedCards = new JoinCollectionFacade<Card, Banlist, SemiLimitedCardBanlistJoin>(this, SemiLimitedCardsJoin);
        }

        /// <summary>
        /// Initialize banlist.
        /// </summary>
        /// <param name="name">Valid name should look like: "YYYY.MM Format" for example "2010 TCG" </param>
        public Banlist(string name)
        {
            Name = name;
            ForbiddenCards = new JoinCollectionFacade<Card, Banlist, ForbiddenCardBanlistJoin>(this, ForbiddenCardsJoin);
            LimitedCards = new JoinCollectionFacade<Card, Banlist, LimitedCardBanlistJoin>(this, LimitedCardsJoin);
            SemiLimitedCards = new JoinCollectionFacade<Card, Banlist, SemiLimitedCardBanlistJoin>(this, SemiLimitedCardsJoin);
        }

        /// <summary>
        /// Id of the banlist.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Valid name should look like: "YYYY.MM Format" for example "2010 TCG" 
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Join property for <see cref="ForbiddenCards"/>
        /// </summary>
        public ICollection<ForbiddenCardBanlistJoin> ForbiddenCardsJoin { get; set; } = new List<ForbiddenCardBanlistJoin>();

        /// <summary>
        /// Join property for <see cref="LimitedCards"/>
        /// </summary>
        public ICollection<LimitedCardBanlistJoin> LimitedCardsJoin { get; set; } = new List<LimitedCardBanlistJoin>();

        /// <summary>
        /// Join property for <see cref="SemiLimitedCards"/>
        /// </summary>
        public ICollection<SemiLimitedCardBanlistJoin> SemiLimitedCardsJoin { get; set; } = new List<SemiLimitedCardBanlistJoin>();

        /// <summary>
        /// List of forbidden cards
        /// </summary>
        [NotMapped]
        public ICollection<Card> ForbiddenCards { get; protected set; }

        /// <summary>
        /// List of limited cards
        /// </summary>
        [NotMapped]
        public ICollection<Card> LimitedCards { get; protected set; }

        /// <summary>
        /// List of semi-limited cards
        /// </summary>
        [NotMapped]
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
        }

        /// <summary>
        /// Release date of the banlist
        /// </summary>
        [NotMapped]
        public DateTime ReleaseDate
        {
            get
            {
                string dateString = Name.Substring(0, Name.IndexOf(' '));
                return DateTime.Parse(dateString,  CultureInfo.InvariantCulture);
            }
        }
    }
}
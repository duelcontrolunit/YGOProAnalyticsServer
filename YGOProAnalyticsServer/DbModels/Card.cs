using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// YuGiOh! card.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Card identifier.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Card name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Card description/effect/flavor text.
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Archetype id.
        /// </summary>
        public int ArchetypeId { get; set; }

        /// <summary>
        /// Card`s archetype.
        /// </summary>
        public Archetype Archetype { get; set; }

        /// <summary>
        /// For example: normal monster, trap card, magic card.
        /// https://db.ygoprodeck.com/api-guide/
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// For example:
        /// <para>1) For monster: aqua, machine warrior</para>
        /// <para>2) For spell: normal, field, quick-spell</para>
        /// <para>3) For trap: normal, continuous, counter</para>
        /// https://db.ygoprodeck.com/api-guide/
        /// </summary>
        public string Race { get; set; }

        /// <summary>
        /// Link to the image of the card.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Link to the small image of the card.
        /// </summary>
        public string SmallImageUrl { get; set; }

        /// <summary>
        /// Join property for <see cref="ForbiddenCards"/>
        /// </summary>
        public ICollection<ForbiddenCardBanlistJoin> ForbiddenCardsJoin { get; } = new List<ForbiddenCardBanlistJoin>();

        /// <summary>
        /// Join property for <see cref="LimitedCards"/>
        /// </summary>
        public ICollection<LimitedCardBanlistJoin> LimitedCardsJoin { get; } = new List<LimitedCardBanlistJoin>();

        /// <summary>
        /// Join property for <see cref="SemiLimitedCards"/>
        /// </summary>
        public ICollection<SemiLimitedCardBanlistJoin> SemiLimitedCardsJoin { get; } = new List<SemiLimitedCardBanlistJoin>();
    }
}

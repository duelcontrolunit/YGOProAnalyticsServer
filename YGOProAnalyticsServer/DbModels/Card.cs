﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database.ManyToManySupport;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// YuGiOh! card. Can be used to store and manage spell or trap card. 
    /// <para>If you want to store or manage monster card use <see cref="MonsterCard"/></para> 
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Include string required to include banlist with forbidden cards.
        /// </summary>
        public static readonly string IncludeWithForbiddenCardsBanlist = $"{nameof(ForbiddenCardsJoin)}.{nameof(ForbiddenCardBanlistJoin.Banlist)}";

        /// <summary>
        /// Include string required to include banlist with limited cards.
        /// </summary>
        public static readonly string IncludeWithLimitedCardsBanlist = $"{nameof(LimitedCardsJoin)}.{nameof(LimitedCardBanlistJoin.Banlist)}";

        /// <summary>
        /// Include string required to include banlist with semi-limited cards
        /// </summary>
        public static readonly string IncludeWithSemiLimitedCardsBanlist = $"{nameof(SemiLimitedCardsJoin)}.{nameof(SemiLimitedCardBanlistJoin.Banlist)}";

        /// <summary>
        /// Adds Basic Card Elements.
        /// </summary>
        /// <param name="passCode">YuGiOh Card Passcode.</param>
        /// <param name="name">Card name.</param>
        /// <param name="description">Card description/effect/flavor text.</param>
        /// <param name="type">
        ///     For example: normal monster, trap card, magic card.
        ///     https://db.ygoprodeck.com/api-guide/
        /// </param>
        /// <param name="race">
        ///     For example:
        ///     <para>1) For monster: aqua, machine warrior</para>
        ///     <para>2) For spell: normal, field, quick-spell</para>
        ///     <para>3) For trap: normal, continuous, counter</para>
        ///     https://db.ygoprodeck.com/api-guide/
        /// </param>
        /// <param name="imageUrl">Link to the image of the card.</param>
        /// <param name="smallImageUrl">Link to the small image of the card.</param>
        public Card(
            int passCode, 
            string name, 
            string description, 
            string type, 
            string race, 
            string imageUrl, 
            string smallImageUrl)
        {
            PassCode = passCode;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Race = race ?? throw new ArgumentNullException(nameof(race));
            ImageUrl = imageUrl;
            SmallImageUrl = smallImageUrl;
            BanlistsWhereThisCardIsForbidden = new JoinCollectionFacade<Banlist, Card, ForbiddenCardBanlistJoin>(this, ForbiddenCardsJoin);
            BanlistsWhereThisCardIsLimited = new JoinCollectionFacade<Banlist, Card, LimitedCardBanlistJoin>(this, LimitedCardsJoin);
            BanlistsWhereThisCardIsSemiLimited = new JoinCollectionFacade<Banlist, Card, SemiLimitedCardBanlistJoin>(this, SemiLimitedCardsJoin);
        }

        /// <summary>
        /// Card identifier.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Id of the card provided from API.
        /// </summary>
        public int PassCode { get; protected set; }

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
        public string Type { get; protected set; }

        /// <summary>
        /// For example:
        /// <para>1) For monster: aqua, machine warrior</para>
        /// <para>2) For spell: normal, field, quick-spell</para>
        /// <para>3) For trap: normal, continuous, counter</para>
        /// https://db.ygoprodeck.com/api-guide/
        /// </summary>
        public string Race { get; protected set; }

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
        public ICollection<ForbiddenCardBanlistJoin> ForbiddenCardsJoin { get; protected set; } = new List<ForbiddenCardBanlistJoin>();

        /// <summary>
        /// Join property for <see cref="LimitedCards"/>
        /// </summary>
        public ICollection<LimitedCardBanlistJoin> LimitedCardsJoin { get; protected set; } = new List<LimitedCardBanlistJoin>();

        /// <summary>
        /// Join property for <see cref="SemiLimitedCards"/>
        /// </summary>
        public ICollection<SemiLimitedCardBanlistJoin> SemiLimitedCardsJoin { get; protected set; } = new List<SemiLimitedCardBanlistJoin>();

        [NotMapped]
        public ICollection<Banlist> BanlistsWhereThisCardIsForbidden { get; protected set; }

        [NotMapped]
        public ICollection<Banlist> BanlistsWhereThisCardIsLimited { get; protected set; }

        [NotMapped]
        public ICollection<Banlist> BanlistsWhereThisCardIsSemiLimited { get; protected set; }
    }
}

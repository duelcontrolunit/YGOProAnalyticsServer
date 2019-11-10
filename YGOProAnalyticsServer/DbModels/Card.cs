using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YGOProAnalyticsServer.Database.ManyToManySupport;
using YGOProAnalyticsServer.DbModels.DbJoinModels;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// YuGiOh! card. Can be used to store and manage spell or trap card. 
    /// <para>If you want to store or manage monster card use:</para> 
    /// <para><see cref="DbModels.MonsterCard"/> for all monsters except links or pendulums,</para>
    /// <para><see cref="LinkMonsterCard"/> for link monsters,</para>
    /// <para><see cref="PendulumMonsterCard"/> for for pendulum monsters.</para>
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
        /// Create card.
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
        /// <param name="archetype">Card archetype</param>
        /// <returns>New Card</returns>
        public static Card Create(
            int passCode,
            string name,
            string description,
            string type,
            string race,
            string imageUrl,
            string smallImageUrl,
            Archetype archetype)
        {
            return new Card(
                passCode,
                name,
                description,
                type,
                race,
                imageUrl,
                smallImageUrl
                )
            {
                Archetype = archetype
            };
        }

        /// <summary>
        /// Changes the pass code.
        /// </summary>
        /// <param name="newPassCode">The new pass code.</param>
        /// <returns>Returns true if operation was successful.</returns>
        public Card ChangePassCode(int newPassCode)
        {
            if (PassCode.ToString().Length > 8)
            {
                PassCode = newPassCode;
                return this;
            }
            return this;
        }

        /// <summary>
        /// Adds Basic Card Elements. Remember to add archetype after creation.
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
        protected Card(
            int passCode,
            string name,
            string description,
            string type,
            string race,
            string imageUrl,
            string smallImageUrl)
        {
            PassCode = passCode;
            Name = name;
            Description = description;
            Type = type;
            Race = race;
            ImageUrl = imageUrl;
            SmallImageUrl = smallImageUrl;
            BanlistsWhereThisCardIsForbidden = new JoinCollectionFacade<Banlist, Card, ForbiddenCardBanlistJoin>(this, ForbiddenCardsJoin);
            BanlistsWhereThisCardIsLimited = new JoinCollectionFacade<Banlist, Card, LimitedCardBanlistJoin>(this, LimitedCardsJoin);
            BanlistsWhereThisCardIsSemiLimited = new JoinCollectionFacade<Banlist, Card, SemiLimitedCardBanlistJoin>(this, SemiLimitedCardsJoin);

            DecksWhereThisCardIsInMainDeck = new JoinCollectionFacade<Decklist, Card, CardInMainDeckDecklistJoin>(this, MainDeckJoin);
            DecksWhereThisCardIsInExtraDeck = new JoinCollectionFacade<Decklist, Card, CardInExtraDeckDecklistJoin>(this, ExtraDeckJoin);
            DecksWhereThisCardIsInSideDeck = new JoinCollectionFacade<Decklist, Card, CardInSideDeckDecklistJoin>(this, SideDeckJoin);
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
        [Required]
        public string Name { get; protected set; }

        /// <summary>
        /// Card description/effect/flavor text.
        /// </summary>
        [Required]
        public string Description { get; protected set; }

        /// <summary>
        /// Card`s archetype.
        /// </summary>
        public Archetype Archetype { get; set; }

        /// <summary>
        /// For example: normal monster, trap card, magic card.
        /// https://db.ygoprodeck.com/api-guide/
        /// </summary>
        [Required]
        public string Type { get; protected set; }

        /// <summary>
        /// For example:
        /// <para>1) For monster: aqua, machine warrior</para>
        /// <para>2) For spell: normal, field, quick-spell</para>
        /// <para>3) For trap: normal, continuous, counter</para>
        /// https://db.ygoprodeck.com/api-guide/
        /// </summary>
        [Required]
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
        /// If card is monster card this should have value. Otherwise it should be null.
        /// </summary>
        public MonsterCard MonsterCard { get; set; }

        /// <summary>
        /// Join property for forbidden <see cref="Card"/>s
        /// </summary>
        public ICollection<ForbiddenCardBanlistJoin> ForbiddenCardsJoin { get; protected set; } = new List<ForbiddenCardBanlistJoin>();

        /// <summary>
        /// Join property for limited <see cref="Card"/>s
        /// </summary>
        public ICollection<LimitedCardBanlistJoin> LimitedCardsJoin { get; protected set; } = new List<LimitedCardBanlistJoin>();

        /// <summary>
        /// Join property for semi-limited <see cref="Card"/>s
        /// </summary>
        public ICollection<SemiLimitedCardBanlistJoin> SemiLimitedCardsJoin { get; protected set; } = new List<SemiLimitedCardBanlistJoin>();
        
        /// <summary>
        /// Banlists where this card is forbidden
        /// </summary>
        [NotMapped]
        public ICollection<Banlist> BanlistsWhereThisCardIsForbidden { get; protected set; }

        /// <summary>
        /// Banlists where this card is limited
        /// </summary>
        [NotMapped]
        public ICollection<Banlist> BanlistsWhereThisCardIsLimited { get; protected set; }

        /// <summary>
        /// Banlists where this cards is semi-limited
        /// </summary>
        [NotMapped]
        public ICollection<Banlist> BanlistsWhereThisCardIsSemiLimited { get; protected set; }

        public ICollection<CardInMainDeckDecklistJoin> MainDeckJoin { get; protected set; } = new List<CardInMainDeckDecklistJoin>();
        public ICollection<CardInExtraDeckDecklistJoin> ExtraDeckJoin { get; protected set; } = new List<CardInExtraDeckDecklistJoin>();
        public ICollection<CardInSideDeckDecklistJoin> SideDeckJoin { get; protected set; } = new List<CardInSideDeckDecklistJoin>();

        [NotMapped]
        public ICollection<Decklist> DecksWhereThisCardIsInMainDeck { get; protected set; }

        [NotMapped]
        public ICollection<Decklist> DecksWhereThisCardIsInExtraDeck { get; protected set; }

        [NotMapped]
        public ICollection<Decklist> DecksWhereThisCardIsInSideDeck { get; protected set; }
    }
}

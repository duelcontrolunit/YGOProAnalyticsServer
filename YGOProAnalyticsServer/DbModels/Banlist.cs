using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
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
        /// <param name="id">Banlist identifier.</param>
        /// <param name="name">Valid name should look like: "YYYY.MM Format" for example "2010 TCG". </param>
        /// <param name="banlistNumberInLfList">Order number in lflist.</param>
        protected Banlist(int id, string name, int banlistNumberInLfList)
        {
            Id = id;
            Name = name;
            BanlistNumberInLfList = banlistNumberInLfList;
            ForbiddenCards = new JoinCollectionFacade<Card, Banlist, ForbiddenCardBanlistJoin>(this, ForbiddenCardsJoin);
            LimitedCards = new JoinCollectionFacade<Card, Banlist, LimitedCardBanlistJoin>(this, LimitedCardsJoin);
            SemiLimitedCards = new JoinCollectionFacade<Card, Banlist, SemiLimitedCardBanlistJoin>(this, SemiLimitedCardsJoin);
        }

        /// <summary>
        /// Initialize banlist.
        /// </summary>
        /// <param name="name">Valid name should look like: "YYYY.MM Format" for example "2010.11 TCG". </param>
        /// /// <param name="banlistNumberInLfList">Order number in lflist.</param>
        public Banlist(string name, int banlistNumberInLfList)
        {
            _validateName(name);
            Name = name;
            BanlistNumberInLfList = banlistNumberInLfList;
            ReleaseDate = GetReleaseDateFromName();
            ForbiddenCards = new JoinCollectionFacade<Card, Banlist, ForbiddenCardBanlistJoin>(this, ForbiddenCardsJoin);
            LimitedCards = new JoinCollectionFacade<Card, Banlist, LimitedCardBanlistJoin>(this, LimitedCardsJoin);
            SemiLimitedCards = new JoinCollectionFacade<Card, Banlist, SemiLimitedCardBanlistJoin>(this, SemiLimitedCardsJoin);
        }

        /// <summary>
        /// Id of the banlist.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Valid name should look like: "YYYY.MM Format" for example "2010.01 TCG".
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the banlist number in lf list.
        /// </summary>
        /// <value>
        /// The banlist number in lf list.
        /// </value>
        public int BanlistNumberInLfList { get; protected set; }

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
        /// List of forbidden cards.
        /// </summary>
        [NotMapped]
        public ICollection<Card> ForbiddenCards { get; protected set; }

        /// <summary>
        /// List of limited cards.
        /// </summary>
        [NotMapped]
        public ICollection<Card> LimitedCards { get; protected set; }

        /// <summary>
        /// List of semi-limited cards.
        /// </summary>
        [NotMapped]
        public ICollection<Card> SemiLimitedCards { get; protected set; }

        /// <summary>
        /// For example TCG, OCG.
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
        /// Release date of the banlist.
        /// </summary>
        public DateTime ReleaseDate { get; protected set; }

        /// <summary>
        /// Collection of the statistics of this banlist.
        /// </summary>
        public ICollection<BanlistStatistics> Statistics { get; protected set; } = new List<BanlistStatistics>();

        /// <summary>
        /// Gets the release date of the banlist.
        /// </summary>
        /// <returns>Release date of the banlist.</returns>
        public DateTime GetReleaseDateFromName()
        {
            string dateString = Name.Substring(0, Name.IndexOf(' '));
            return DateTime.Parse(dateString, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Validates the name of the banlist.
        /// </summary>
        /// <param name="name">The name of the banlist</param>
        /// <exception cref="System.FormatException"></exception>
        private void _validateName(string name)
        {
            if(!Regex.IsMatch(name, @"^\d{4}\.\d{2} \w{1,}"))
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Invalid name format. Valid name format is 'YYYY.MM YuGiOhFormat'.");
                stringBuilder.AppendLine($"Given name: {name}");

                throw new FormatException(stringBuilder.ToString());
            }
        }
    }
}
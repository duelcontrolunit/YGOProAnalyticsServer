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
        public Banlist(
            int id,
            string name)
        {
            Id = id;
            Name = name;
            ForbiddenCards = new JoinCollectionFacade<Card, Banlist, BanlistCardJoin>(this, ForbiddenCardsJoin);
            LimitedCards = new JoinCollectionFacade<Card, Banlist, BanlistCardJoin>(this, LimitedCardsJoin); ;
            SemiLimitedCards = new JoinCollectionFacade<Card, Banlist, BanlistCardJoin>(this, SemiLimitedCardsJoin); ;
        }


        /// <summary>
        /// Id of the banlist
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Name of the banlist
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Join property for <see cref="ForbiddenCards"/>
        /// </summary>
        public ICollection<BanlistCardJoin> ForbiddenCardsJoin = new List<BanlistCardJoin>();

        /// <summary>
        /// Join property for <see cref="LimitedCards"/>
        /// </summary>
        public ICollection<BanlistCardJoin> LimitedCardsJoin = new List<BanlistCardJoin>();

        /// <summary>
        /// Join property for <see cref="SemiLimitedCards"/>
        /// </summary>
        public ICollection<BanlistCardJoin> SemiLimitedCardsJoin = new List<BanlistCardJoin>();

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
            protected set { }
        }
    }
}
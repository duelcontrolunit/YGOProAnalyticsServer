using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database.ManyToManySupport;

namespace YGOProAnalyticsServer.DbModels.DbJoinModels
{
    /// <summary>
    /// Join dbModel between <see cref="DbModels.Decklist"/> and <see cref="DbModels.Banlist"/>
    /// </summary>
    public class DecklistsBanlistsJoin : IJoinEntity<Decklist>, IJoinEntity<Banlist>
    {
        /// <summary>
        /// Gets or sets the decklist identifier.
        /// </summary>
        /// <value>
        /// The decklist identifier.
        /// </value>
        public int DecklistId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        public Decklist Decklist { get; set; }

        /// <summary>
        /// It exposing navigation property for <see cref="JoinCollectionFacade{TEntity, TOtherEntity, TJoinEntity}  use."/>
        /// </summary>
        Decklist IJoinEntity<Decklist>.Navigation { get => Decklist; set => Decklist = value; }

        /// <summary>
        /// Gets or sets the banlist identifier.
        /// </summary>
        /// <value>
        /// The banlist identifier.
        /// </value>
        public int BanlistId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        public Banlist Banlist { get; set; }

        /// <summary>
        /// It exposing navigation property for <see cref="JoinCollectionFacade{TEntity, TOtherEntity, TJoinEntity}  use."/>
        /// </summary>
        Banlist IJoinEntity<Banlist>.Navigation { get => Banlist; set => Banlist = value; }
    }
}

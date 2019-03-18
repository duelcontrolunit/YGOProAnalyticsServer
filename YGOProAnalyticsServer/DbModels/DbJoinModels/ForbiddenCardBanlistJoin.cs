using YGOProAnalyticsServer.Database.ManyToManySupport;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// Join dbModel between <see cref="DbModels.Card"/> and <see cref="DbModels.Banlist"/>
    /// </summary>
    public class ForbiddenCardBanlistJoin : IJoinEntity<Card>, IJoinEntity<Banlist>
    {
        /// <summary>
        /// Id of the card.
        /// </summary>
        public int CardId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        public Card Card { get; set; }

        /// <summary>
        /// It exposing navigation property for <see cref="JoinCollectionFacade{TEntity, TOtherEntity, TJoinEntity}  use."/>
        /// </summary>
        Card IJoinEntity<Card>.Navigation
        {
            get => Card;
            set => Card = value;
        }

        /// <summary>
        /// Id of the balist.
        /// </summary>
        public int BanlistId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        public Banlist Banlist { get; set; }

        /// <summary>
        /// It exposing navigation property for <see cref="JoinCollectionFacade{TEntity, TOtherEntity, TJoinEntity} use."/>
        /// </summary>
        Banlist IJoinEntity<Banlist>.Navigation
        {
            get => Banlist;
            set => Banlist = value;
        }
    }
}

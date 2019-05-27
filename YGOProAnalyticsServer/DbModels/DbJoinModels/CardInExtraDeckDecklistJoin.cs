using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database.ManyToManySupport;

namespace YGOProAnalyticsServer.DbModels.DbJoinModels
{
    public class CardInExtraDeckDecklistJoin : CardDecklistJoinAbstract
    {
    }

    //public class CardInExtraDeckDecklistJoin : IJoinEntity<Card>, IJoinEntity<Decklist>
    //{
    //    public int CardId { get; set; }
    //    public Card Card { get; set; }
    //    public Card Navigation { get => Card; set => Card = value; }

    //    public int DecklistId { get; set; }
    //    public Decklist Decklist { get; set; }
    //    Decklist IJoinEntity<Decklist>.Navigation { get => Decklist; set => Decklist = value; }
    //}
}

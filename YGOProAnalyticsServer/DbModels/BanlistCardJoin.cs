using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database.ManyToManySupport;

namespace YGOProAnalyticsServer.DbModels
{
    public class BanlistCardJoin : IJoinEntity<Card>, IJoinEntity<Banlist>
    {
        public int CardId { get; set; }
        public Card Card { get; set; }
        Card IJoinEntity<Card>.Navigation
        {
            get => Card;
            set => Card = value;
        }

        public int BanlistId { get; set; }
        public Banlist Banlist { get; set; }
        Banlist IJoinEntity<Banlist>.Navigation
        {
            get => Banlist;
            set => Banlist = value;
        }
    }
}

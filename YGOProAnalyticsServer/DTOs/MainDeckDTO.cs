using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.DTOs
{
    public class MainDeckDTO
    {
        public List<Card> NormalMonsters { get; set; }
        public List<Card> EffectMonsters { get; set; }
        public List<Card> PendulumNormalMonsters { get; set; }
        public List<Card> PendulumEffectMonsters { get; set; }
        public List<Card> RitualMonsters { get; set; }
        public List<Card> SpellCards { get; set; }
        public List<Card> TrapCards { get; set; }
    }
}

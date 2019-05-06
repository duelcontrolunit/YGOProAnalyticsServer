using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.DTOs
{
    public class DeckDTO
    {
        public List<CardDTO> NormalMonsters { get; set; }
        public List<CardDTO> EffectMonsters { get; set; }
        public List<CardDTO> PendulumNormalMonsters { get; set; }
        public List<CardDTO> PendulumEffectMonsters { get; set; }
        public List<CardDTO> RitualMonsters { get; set; }
        public List<CardDTO> SpellCardDTOs { get; set; }
        public List<CardDTO> TrapCardDTOs { get; set; }
        public List<CardDTO> XYZMonsters { get; set; }
        public List<CardDTO> XYZPendulumMonsters { get; set; }
        public List<CardDTO> SynchroMonsters { get; set; }
        public List<CardDTO> SynchroPendulumMonsters { get; set; }
        public List<CardDTO> FusionMonsters { get; set; }
        public List<CardDTO> FusionPendulumMonsters { get; set; }
        public List<CardDTO> LinkMonsters { get; set; }
    }
}

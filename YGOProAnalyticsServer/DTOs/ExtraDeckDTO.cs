using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.DTOs
{
    public class ExtraDeckDTO
    {
        public List<Card> XYZMonsters { get; set; }
        public List<Card> XYZPendulumMonsters { get; set; }
        public List<Card> SynchroMonsters { get; set; }
        public List<Card> SynchroPendulumMonsters { get; set; }
        public List<Card> FusionMonsters { get; set; }
        public List<Card> FusionPendulumMonsters { get; set; }
        public List<Card> LinkMonsters { get; set; }
    }
}

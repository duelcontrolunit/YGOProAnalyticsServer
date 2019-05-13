using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs.Interfaces;

namespace YGOProAnalyticsServer.DTOs
{
    public class ExtraDeckDTO : IXYZPendulumContainer, ISynchroPendulumMonsterContainer,
        IFusionPendulumMonsterContainer, IXYZMonstersContainer, ISynchroMonstersContainer,
        IFusionMonstersContainer, ILinkMonstersContainer
    {
        public List<MonsterCardDTO> XYZMonsters { get; set; } = new List<MonsterCardDTO>();
        public List<PendulumMonsterCardDTO> XYZPendulumMonsters { get; set; } = new List<PendulumMonsterCardDTO>();
        public List<MonsterCardDTO> SynchroMonsters { get; set; } = new List<MonsterCardDTO>();
        public List<PendulumMonsterCardDTO> SynchroPendulumMonsters { get; set; } = new List<PendulumMonsterCardDTO>();
        public List<MonsterCardDTO> FusionMonsters { get; set; } = new List<MonsterCardDTO>();
        public List<PendulumMonsterCardDTO> FusionPendulumMonsters { get; set; } = new List<PendulumMonsterCardDTO>();
        public List<LinkMonsterCardDTO> LinkMonsters { get; set; } = new List<LinkMonsterCardDTO>();
    }
}

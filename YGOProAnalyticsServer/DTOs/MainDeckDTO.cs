using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs.Interfaces;

namespace YGOProAnalyticsServer.DTOs
{
    public class MainDeckDTO : ISpellCardsContainer, 
        ITrapCardsContainer, IRitualMonstersContainer, 
        INormalMonstersContainer, IEffectMonsterContainer,
        INonExtraPendulumNormalMonster, INonExtraPendulumEffectMonster
    {
        public List<MonsterCardDTO> NormalMonsters { get; set; } = new List<MonsterCardDTO>();
        public List<MonsterCardDTO> EffectMonsters { get; set; } = new List<MonsterCardDTO>();
        public List<PendulumMonsterCardDTO> PendulumNormalMonsters { get; set; } = new List<PendulumMonsterCardDTO>();
        public List<PendulumMonsterCardDTO> PendulumEffectMonsters { get; set; } = new List<PendulumMonsterCardDTO>();
        public List<MonsterCardDTO> RitualMonsters { get; set; } = new List<MonsterCardDTO>();
        public List<CardDTO> SpellCards { get; set; } = new List<CardDTO>();
        public List<CardDTO> TrapCards { get; set; } = new List<CardDTO>();
    }
}

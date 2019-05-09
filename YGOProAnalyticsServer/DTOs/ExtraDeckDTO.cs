﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.DTOs
{
    public class ExtraDeckDTO
    {
        public List<CardDTO> XYZMonsters { get; set; } = new List<CardDTO>();
        public List<PendulumMonsterCardDTO> XYZPendulumMonsters { get; set; } = new List<PendulumMonsterCardDTO>();
        public List<MonsterCardDTO> SynchroMonsters { get; set; } = new List<MonsterCardDTO>();
        public List<PendulumMonsterCardDTO> SynchroPendulumMonsters { get; set; } = new List<PendulumMonsterCardDTO>();
        public List<MonsterCardDTO> FusionMonsters { get; set; } = new List<MonsterCardDTO>();
        public List<PendulumMonsterCardDTO> FusionPendulumMonsters { get; set; } = new List<PendulumMonsterCardDTO>();
        public List<LinkMonsterCardDTO> LinkMonsters { get; set; } = new List<LinkMonsterCardDTO>();
    }
}

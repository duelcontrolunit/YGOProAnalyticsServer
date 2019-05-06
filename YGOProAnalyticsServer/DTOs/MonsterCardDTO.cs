using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class MonsterCardDTO : CardDTO
    {
        public string Attack { get; set; }
        public string Defence { get; set; }
        public int LevelOrRank { get; set; }
        public string Attribute { get; set; }
    }
}

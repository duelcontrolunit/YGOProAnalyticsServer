using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels
{
    public class PendulumMonsterCard
    {
        public static PendulumMonsterCard Create(
            int scale,
            MonsterCard monsterCard)
        {
            return new PendulumMonsterCard(scale)
            {
                MonsterCard = monsterCard
            };
        }
        public PendulumMonsterCard(int scale)
        {
            Scale = scale;
        }

        public int Id { get; protected set; }
        /// <summary>
        /// Scale value. Only pendulum monsters should have it.
        /// </summary>
        public int Scale { get; protected set; }
        public int MonsterCardId { get; set; }
        public MonsterCard MonsterCard { get; set; }
    }
}

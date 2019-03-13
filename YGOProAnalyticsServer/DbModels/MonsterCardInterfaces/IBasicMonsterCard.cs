using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels.MonsterCardInterfaces
{
    public interface IBasicMonsterCard
    {
        void SetOrUpdateMonsterCardBasicProperties(
            int passCode,
            string name,
            string description,
            string type,
            string race,
            string imageUrl,
            string smallImageUrl,
            string attack,
            string defence,
            int levelOrRank,
            string attribute,
            Archetype archetype);
    }
}

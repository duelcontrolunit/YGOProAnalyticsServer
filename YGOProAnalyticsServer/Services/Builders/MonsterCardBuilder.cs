using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DbModels.MonsterCardInterfaces;
using YGOProAnalyticsServer.Exceptions;
using YGOProAnalyticsServer.Services.Builders.Interfaces;

namespace YGOProAnalyticsServer.Services.Builders
{
    public class MonsterCardBuilder : IMonsterCardBuilder
    {
        MonsterCard _monsterCard;
        bool _isMonsterProperlyInitialized = false;

        public MonsterCardBuilder AddBasicMonsterElements(
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
            Archetype archetype)
        {
            _initMonsterCardIfRequired();
            ((IBasicMonsterCard)_monsterCard)
                .SetOrUpdateMonsterCardBasicProperties(
                    passCode,
                    name,
                    description,
                    type,
                    race,
                    imageUrl,
                    smallImageUrl,
                    attack,
                    defence,
                    levelOrRank,
                    archetype
                );
            _isMonsterProperlyInitialized = true;
            return this;
        }

        public MonsterCardBuilder AddPendulumElements(int scale)
        {
            _initMonsterCardIfRequired();
            ((IPendulumMonster)_monsterCard).SetOrUpdateScale(scale);
            return this;
        }

        public MonsterCardBuilder AddLinkElements(
            int linkValue,
            bool topLeftLinkMarker,
            bool topLinkMarker,
            bool topRightLinkMarker,
            bool middleLeftLinkMarker,
            bool middleRightLinkMarker,
            bool bottomLeftLinkMarker,
            bool bottomLinkMarker,
            bool bottomRightLinkMarker)
        {
            _initMonsterCardIfRequired();
            ((ILinkMonster)_monsterCard)
                .SetOrUpdateLinkElements(
                    linkValue,
                    topLeftLinkMarker,
                    topLinkMarker,
                    topRightLinkMarker,
                    middleLeftLinkMarker,
                    middleRightLinkMarker,
                    bottomLeftLinkMarker,
                    bottomLinkMarker,
                    bottomRightLinkMarker
                );
            return this;
        }

        public MonsterCard Build()
        {
            if (!_isMonsterProperlyInitialized)
                throw new NotProperlyInitialized($"You must call ${nameof(AddBasicMonsterElements)} before you call ${nameof(Build)}.");
            return _monsterCard;
        }

        private void _initMonsterCardIfRequired()
        {
            _monsterCard = _monsterCard ?? MonsterCard.GetInstanceForBuilder();
        }
    }
}

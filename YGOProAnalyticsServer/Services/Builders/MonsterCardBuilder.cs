using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DbModels.MonsterCardInterfaces;
using YGOProAnalyticsServer.Exceptions;
using YGOProAnalyticsServer.Services.Builders.Interfaces;

namespace YGOProAnalyticsServer.Services.Builders
{
    /// <summary>
    /// Builder made for creating <see cref="MonsterCard"/>
    /// </summary>
    public class MonsterCardBuilder : IMonsterCardBuilder
    {
        MonsterCard _monsterCard;
        bool _isMonsterProperlyInitialized = false;
        bool _wasBuilt = false;

        /// <summary>
        /// Adds Basic Monster Elements. REQUIRED FOR ALL MONSTERS.
        /// </summary>
        /// <param name="passCode">YuGiOh Card Passcode.</param>
        /// <param name="name">Card name.</param>
        /// <param name="description">Card description/effect/flavor text.</param>
        /// <param name="type">
        ///     For example: normal monster, trap card, magic card.
        ///     https://db.ygoprodeck.com/api-guide/
        /// </param>
        /// <param name="race">
        ///     For example:
        ///     <para>1) For monster: aqua, machine warrior</para>
        ///     <para>2) For spell: normal, field, quick-spell</para>
        ///     <para>3) For trap: normal, continuous, counter</para>
        ///     https://db.ygoprodeck.com/api-guide/
        /// </param>
        /// <param name="imageUrl">Link to the image of the card.</param>
        /// <param name="smallImageUrl">Link to the small image of the card.</param>
        /// <param name="attack">Attack value of the Monster.</param>
        /// <param name="defence">Defence value of the Monster.</param>
        /// <param name="levelOrRank">Level or Rank of the Monster. For Links it should be 0</param>
        /// <param name="archetype">Archetype of the Monster</param>
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

        /// <summary>
        /// Adds Pendulum elements used by Pendulum Monsters.
        /// </summary>
        /// <param name="scale">Pendulum Scale of the Monster</param>
        public MonsterCardBuilder AddPendulumElements(int scale)
        {
            _initMonsterCardIfRequired();
            ((IPendulumMonster)_monsterCard).SetOrUpdateScale(scale);
            return this;
        }
        /// <summary>
        /// Adds Link elements used by Link Monsters.
        /// </summary>
        /// <param name="linkValue">Link Value of the monster.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Builds and returns the MonsterCard.
        /// </summary>
        /// <exception cref="NotProperlyInitializedException">Thrown when AddBasicMonsterElements method was not called.</exception>
        /// <returns>returns built MonsterCards</returns>
        public MonsterCard Build()
        {
            if (!_isMonsterProperlyInitialized)
                throw new NotProperlyInitializedException($"You must call ${nameof(AddBasicMonsterElements)} before you call ${nameof(Build)}.");
            _wasBuilt = true;
            return _monsterCard;
        }

        private void _initMonsterCardIfRequired()
        {
            if (_wasBuilt)
            {
                _monsterCard = MonsterCard.GetInstanceForBuilder();
                _wasBuilt = false;
            }
            else
            {
                _monsterCard = _monsterCard ?? MonsterCard.GetInstanceForBuilder();
            }
        }
    }
}

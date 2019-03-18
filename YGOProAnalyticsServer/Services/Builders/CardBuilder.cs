using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Exceptions;
using YGOProAnalyticsServer.Services.Builders.Inferfaces;

namespace YGOProAnalyticsServer.Services.Builders
{
    /// <summary>
    /// It provide methods to properly build different types of cards.
    /// </summary>
    /// <seealso cref="YGOProAnalyticsServer.Services.Builders.Inferfaces.ICardBuilder" />
    public class CardBuilder : ICardBuilder
    {
        Card _card;
        MonsterCard _monsterCard;
        LinkMonsterCard _linkMonsterCard;
        PendulumMonsterCard _pendulumMonsterCard; 

        /// <<inheritdoc />
        public CardBuilder AddBasicCardElements(
                int passCode,
                string name,
                string description,
                string type,
                string race,
                string imageUrl,
                string smallImageUrl,
                Archetype archetype
                )
        {
            _card = Card.Create(
                passCode,
                name,
                description,
                type,
                race,
                imageUrl,
                smallImageUrl,
                archetype
                );
            return this;
        }

        /// <<inheritdoc />
        public CardBuilder AddMonsterCardElements(
            string attack,
            string defence,
            int levelOrRank,
            string attribute
            )
        {
            _monsterCard = MonsterCard.Create(
                attack,
                defence,
                levelOrRank,
                attribute,
                _card
                );
            return this;
        }

        /// <<inheritdoc />
        public CardBuilder AddLinkMonsterCardElements(
            int linkValue,
            bool topLeftLinkMarker,
            bool topLinkMarker,
            bool topRightLinkMarker,
            bool middleLeftLinkMarker,
            bool middleRightLinkMarker,
            bool bottomLeftLinkMarker,
            bool bottomLinkMarker,
            bool bottomRightLinkMarker
            )
        {
            _linkMonsterCard = LinkMonsterCard.Create(
                linkValue,
                topLeftLinkMarker,
                topLinkMarker,
                topRightLinkMarker,
                middleLeftLinkMarker,
                middleRightLinkMarker,
                bottomLeftLinkMarker,
                bottomLinkMarker,
                bottomRightLinkMarker,
                _monsterCard
                );
            return this;
        }

        /// <<inheritdoc />
        public CardBuilder AddPendulumMonsterCardElements(int scale)
        {
            _pendulumMonsterCard = PendulumMonsterCard.Create(scale, _monsterCard);
            return this;
        }

        /// <<inheritdoc />
        public Card Build()
        {
            if (_card == null)
            {
                throw new NotProperlyInitializedException($"Card cannot be null. Call {nameof(AddBasicCardElements)} before {nameof(Build)}");
            }
            
            if (_pendulumMonsterCard != null)
            {
                CheckIfMonsterInitialized(_monsterCard);
                _monsterCard.PendulumMonsterCard = _pendulumMonsterCard;
                _pendulumMonsterCard.MonsterCard = _monsterCard;
            }

            if (_linkMonsterCard != null) {
                CheckIfMonsterInitialized(_monsterCard);
                _monsterCard.LinkMonsterCard = _linkMonsterCard;
                _linkMonsterCard.MonsterCard = _monsterCard;
            }

            if (_monsterCard != null)
            {
                _monsterCard.Card = _card;
                _card.MonsterCard = _monsterCard;
            }
            _monsterCard = null;
            _linkMonsterCard = null;
            _pendulumMonsterCard = null;

            return _card;
        }

        private void CheckIfMonsterInitialized(MonsterCard monsterCard)
        {
            if (monsterCard == null)
            {
                throw new NotProperlyInitializedException($"Monster cannot be null when adding additional monster elements. Call {nameof(AddMonsterCardElements)}.");
            }
        }
    }
}

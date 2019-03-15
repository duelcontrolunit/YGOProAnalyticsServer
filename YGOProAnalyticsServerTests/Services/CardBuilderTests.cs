using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Exceptions;
using YGOProAnalyticsServer.Services.Builders;
using YGOProAnalyticsServer.Services.Builders.Inferfaces;

namespace YGOProAnalyticsServerTests.Services
{
    [TestFixture]
    class CardBuilderTests
    {
        ICardBuilder _cardBuilder;
        Archetype _archetype = new Archetype("Test", true);
        [SetUp]
        public void Setup()
        {
            _cardBuilder = new CardBuilder();
        }

        [TestCaseSource(nameof(BasicCardElements))]
        public void AddBasicCardElements_AllDataValid_ReturnsCard(
            int passCode,
            string name,
            string description,
            string type,
            string race,
            string imageUrl,
            string smallImageUrl)
        {
            _cardBuilder.AddBasicCardElements(passCode,
                name,
                description,
                type,
                race,
                imageUrl,
                smallImageUrl,
                _archetype);
            Assert.NotNull(_cardBuilder.Build());
        }

        [TestCaseSource(nameof(BasicAndMonsterCardElements))]
        public void AddMonsterCardElements_AllDataValid_ReturnsCardWithNotNullMonsterCard(
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
            string attribute)
        {
            _cardBuilder.AddBasicCardElements(passCode,
                name,
                description,
                type,
                race,
                imageUrl,
                smallImageUrl,
                _archetype);

            _cardBuilder.AddMonsterCardElements(
                attack,
                defence,
                levelOrRank,
                attribute);

            Assert.NotNull(_cardBuilder.Build().MonsterCard);
        }

        [TestCaseSource(nameof(BasicAndMonsterAndPendulumMonsterCardElements))]
        public void AddPendulumMonsterCardElements_AllDataValid_ReturnsMonsterCardWithNotNullPendulumMonster(
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
            int scale)
        {
            _cardBuilder.AddBasicCardElements(passCode,
                name,
                description,
                type,
                race,
                imageUrl,
                smallImageUrl,
                _archetype);

            _cardBuilder.AddMonsterCardElements(
                attack,
                defence,
                levelOrRank,
                attribute);

            _cardBuilder.AddPendulumMonsterCardElements(scale);

            Assert.NotNull(_cardBuilder.Build().MonsterCard.PendulumMonsterCard);
        }

        [TestCaseSource(nameof(BasicAndMonsterAndLinkMonsterCardElements))]
        public void AddLinkMonsterCardElements_AllDataValid_ReturnsMonsterCardWithNotNullLinkMonster(
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
            _cardBuilder.AddBasicCardElements(passCode,
                name,
                description,
                type,
                race,
                imageUrl,
                smallImageUrl,
                _archetype);

            _cardBuilder.AddMonsterCardElements(
                attack,
                defence,
                levelOrRank,
                attribute);

            _cardBuilder.AddLinkMonsterCardElements(
                linkValue,
                topLeftLinkMarker,
                topLinkMarker,
                topRightLinkMarker,
                middleLeftLinkMarker,
                middleRightLinkMarker,
                bottomLeftLinkMarker,
                bottomLinkMarker,
                bottomRightLinkMarker);

            Assert.NotNull(_cardBuilder.Build().MonsterCard.LinkMonsterCard);
        }

        [Test]
        public void Build_MissingBasicCardElements_throwsNotProperlyInitializedException()
        { 
            Assert.Throws<NotProperlyInitializedException>(() => { _cardBuilder.Build(); });
        }

        [TestCaseSource(nameof(BasicAndMonsterAndPendulumMonsterCardElements))]
        public void BuildPendulumMonster_MissingMonsterElements_ThrowsNotProperlyInitializedException(
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
            int scale
            )
        {
            _cardBuilder.AddBasicCardElements(passCode,
                name,
                description,
                type,
                race,
                imageUrl,
                smallImageUrl,
                _archetype);

            _cardBuilder.AddPendulumMonsterCardElements(scale);

            Assert.Throws<NotProperlyInitializedException>(() => { _cardBuilder.Build(); });
        }

        public void BuildLinkMonsterCard_MissingMonsterElements_ThrowsNotProperlyInitializedException(
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
            _cardBuilder.AddBasicCardElements(passCode,
                name,
                description,
                type,
                race,
                imageUrl,
                smallImageUrl,
                _archetype);

            _cardBuilder.AddLinkMonsterCardElements(
                linkValue,
                topLeftLinkMarker,
                topLinkMarker,
                topRightLinkMarker,
                middleLeftLinkMarker,
                middleRightLinkMarker,
                bottomLeftLinkMarker,
                bottomLinkMarker,
                bottomRightLinkMarker);

            Assert.Throws<NotProperlyInitializedException>(() => { _cardBuilder.Build(); });
        }

        static object[] BasicCardElements =
        {
            new object[] { 98, "Dragon", "A wild monster appears!", "Normal Monster", "Blue Dragon", string.Empty, string.Empty },
            new object[] { 99, "Mage", "Old, bald.", "Effect Monster", "Grey Mage", string.Empty, string.Empty }
        };
        static object[] BasicAndMonsterCardElements =
        {
            new object[] { 98, "Dragon", "A wild monster appears!", "Normal Monster", "Blue Dragon", string.Empty, string.Empty, "3000", "2500", 8, "Light" },
            new object[] { 99, "Mage", "Old, bald.", "Effect Monster", "Grey Mage", string.Empty, string.Empty, "2500", "2100", 7, "Dark"}
        };
        static object[] BasicAndMonsterAndPendulumMonsterCardElements =
        {
            new object[] { 98, "Dragon", "A wild monster appears!", "Pendulum Normal Monster", "Blue Dragon", string.Empty, string.Empty, "3000", "2500", 8, "Light", 1 },
            new object[] { 99, "Mage", "Old, bald.", "Pendulum Effect Monster", "Grey Mage", string.Empty, string.Empty, "2500", "2100", 7, "Dark", 9}
        };
        static object[] BasicAndMonsterAndLinkMonsterCardElements =
        {
            new object[] { 98, "Dragon", "A wild monster appears!", "Link Monster", "Blue Dragon", string.Empty, string.Empty, "3000", "2500", 8, "Light", 8, true, true, true, true, true, true, true, true },
            new object[] { 99, "Mage", "Old, bald.", "Link Effect Monster", "Grey Mage", string.Empty, string.Empty, "2500", "2100", 7, "Dark", 1, false, false, false, false, false, false, false, true }
        };
    }
}

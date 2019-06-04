using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServerTests.TestingHelpers
{
    class CardsAndDecksHelper
    {
        public Decklist GetValidDecklistWithStatistics(Archetype archetype)
        {
            var decklist = new Decklist(new List<Card>(), new List<Card>(), new List<Card>()) {
                Name = "Valid decklist",
                Archetype = archetype,
                WhenDecklistWasFirstPlayed = new DateTime(1997, 4, 29)
            };
            decklist.DecklistStatistics.Add(GetValidStatisticsForValidDeck());

            return decklist;
        }

        public PendulumMonsterCard GetPendulumMonsterCard(MonsterCard monster)
        {
            return PendulumMonsterCard.Create(5, monster);
        }

        public LinkMonsterCard GetLinkMonsterCard(MonsterCard monster)
        {
            return LinkMonsterCard.Create(
                1,
                topLeftLinkMarker: true,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                monster);
        }

        public Card GetCard(Archetype archetype, string type = "default type")
        {
            return Card.Create(
                passCode: 12345,
                name: "monster name",
                description: "monster desc",
                type: type,
                race: "monster race",
                imageUrl: "imgUrl",
                smallImageUrl: "smallImgUrl",
                archetype
            );
        }

        public MonsterCard GetMonsterCard(Card card)
        {
            return MonsterCard.Create(
                attack: "1234",
                defence: "?",
                levelOrRank: 5,
                attribute: "dark",
                card: card
            );
        }

        public DecklistStatistics GetValidStatisticsForValidDeck()
        {
            DecklistStatistics statistics = new DecklistStatistics();
            statistics.IncrementNumberOfTimesWhenDeckWasUsed();

            statistics.GetType()
                .GetProperty(nameof(DecklistStatistics.DateWhenDeckWasUsed))
                .SetValue(statistics, new DateTime(1997, 4, 29));

            statistics.GetType()
               .GetProperty(nameof(DecklistStatistics.Id))
               .SetValue(statistics, 1);

            return statistics;
        }
    }
}

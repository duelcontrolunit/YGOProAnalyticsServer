using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Factories.Interfaces;

namespace YGOProAnalyticsServer.Services.Factories
{
    public class CardDtosFactory : ICardDtosFactory
    {
        public virtual CardDTO CreateCardDto(Card card)
        {
            return new CardDTO()
            {
                PassCode = card.PassCode,
                Type = card.Type,
                Description = card.Description,
                Name = card.Name,
                Race = card.Race,
                Archetype = card.Archetype.Name,
                ImageUrl = card.ImageUrl,
                SmallImageUrl = card.SmallImageUrl
            };
        }

        public virtual MonsterCardDTO CreateMonsterCardDto(Card card)
        {
            return new MonsterCardDTO()
            {
                PassCode = card.PassCode,
                Type = card.Type,
                Description = card.Description,
                Name = card.Name,
                Race = card.Race,
                Archetype = card.Archetype.Name,
                ImageUrl = card.ImageUrl,
                SmallImageUrl = card.SmallImageUrl,
                Attack = card.MonsterCard.Attack,
                Defence = card.MonsterCard.Defence,
                Attribute = card.MonsterCard.Attribute,
                LevelOrRank = card.MonsterCard.LevelOrRank
            };
        }

        public virtual PendulumMonsterCardDTO CreatePendulumMonsterCardDto(Card card)
        {
            return new PendulumMonsterCardDTO()
            {
                PassCode = card.PassCode,
                Type = card.Type,
                Description = card.Description,
                Name = card.Name,
                Race = card.Race,
                Archetype = card.Archetype.Name,
                ImageUrl = card.ImageUrl,
                SmallImageUrl = card.SmallImageUrl,
                Attack = card.MonsterCard.Attack,
                Defence = card.MonsterCard.Defence,
                Attribute = card.MonsterCard.Attribute,
                LevelOrRank = card.MonsterCard.LevelOrRank,
                Scale = card.MonsterCard.PendulumMonsterCard.Scale
            };
        }
    }
}

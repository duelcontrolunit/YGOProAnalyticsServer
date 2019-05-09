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

        public virtual LinkMonsterCardDTO CreateLinkMonsterCardDto(Card card)
        {
            return new LinkMonsterCardDTO()
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
                BottomLeftLinkMarker = card.MonsterCard.LinkMonsterCard.BottomLeftLinkMarker,
                BottomLinkMarker = card.MonsterCard.LinkMonsterCard.BottomLinkMarker,
                BottomRightLinkMarker = card.MonsterCard.LinkMonsterCard.BottomRightLinkMarker,
                LinkValue = card.MonsterCard.LinkMonsterCard.LinkValue,
                MiddleLeftLinkMarker = card.MonsterCard.LinkMonsterCard.MiddleLeftLinkMarker,
                MiddleRightLinkMarker = card.MonsterCard.LinkMonsterCard.MiddleRightLinkMarker,
                TopLeftLinkMarker = card.MonsterCard.LinkMonsterCard.TopLeftLinkMarker,
                TopLinkMarker = card.MonsterCard.LinkMonsterCard.TopLinkMarker,
                TopRightLinkMarker = card.MonsterCard.LinkMonsterCard.TopRightLinkMarker
            };
        }
    }
}

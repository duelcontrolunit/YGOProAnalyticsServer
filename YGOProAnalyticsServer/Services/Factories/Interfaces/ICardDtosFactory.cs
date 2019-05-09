using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Factories.Interfaces
{
    public interface ICardDtosFactory
    {
        CardDTO CreateCardDto(Card card);
        MonsterCardDTO CreateMonsterCardDto(Card card);
        PendulumMonsterCardDTO CreatePendulumMonsterCardDto(Card card);
    }
}
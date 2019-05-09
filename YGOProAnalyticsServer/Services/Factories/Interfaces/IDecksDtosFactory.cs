using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Factories.Interfaces
{
    public interface IDecksDtosFactory
    {
        MainDeckDTO CreateMainDeckDto(Decklist decklist);
        ExtraDeckDTO CreateExtraDeckDto(Decklist decklist);
        DeckDTO CreateDeckDto(Decklist decklist);
    }
}
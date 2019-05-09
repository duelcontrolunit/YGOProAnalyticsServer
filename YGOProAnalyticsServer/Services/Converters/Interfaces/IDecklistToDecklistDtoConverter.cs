using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Converters.Interfaces
{
    public interface IDecklistToDecklistDtoConverter
    {
        DecklistWithStatisticsDTO Convert(Decklist banlist);
    }
}
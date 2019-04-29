using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Converters.Interfaces
{
    public interface IYDKToDecklistConverter
    {
        Decklist Convert(string ydkString);
    }
}
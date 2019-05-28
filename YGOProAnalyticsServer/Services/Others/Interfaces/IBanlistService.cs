using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Others.Interfaces
{
    public interface IBanlistService
    {
        bool CanDeckBeUsedOnGivenBanlist(Decklist decklist, Banlist banlist);
    }
}
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Services.Updaters.Interfaces
{
    public interface ICardsDataToCardsAndArchetypesUpdater
    {
        Task ConvertCards(string URL);
    }
}
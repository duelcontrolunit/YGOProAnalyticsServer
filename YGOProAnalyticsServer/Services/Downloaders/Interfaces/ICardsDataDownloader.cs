using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Services.Downloaders.Interfaces
{
    public interface ICardsDataDownloader
    {
        Task<string> DownloadCardsFromWebsite(string EndPointURL);
    }
}
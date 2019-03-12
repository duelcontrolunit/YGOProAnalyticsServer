using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Services.Downloaders.Interfaces
{
    public interface IBanlistDataDownloader
    {
        Task<string> DownloadBanlistFromWebsite(string websiteURL);
    }
}
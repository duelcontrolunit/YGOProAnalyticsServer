using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Services.Downloaders.Interfaces
{
    public interface IDuelLogDownloader
    {
        Task<string> DownloadDuelLogFromFTP(string EndPointFTP);
    }
}
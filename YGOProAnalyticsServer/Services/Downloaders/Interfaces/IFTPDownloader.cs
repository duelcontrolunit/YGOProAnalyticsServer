using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Services.Downloaders.Interfaces
{
    /// <summary>Provides methods to download DuelLogFromFTP</summary>
    public interface IFTPDownloader
    {
        /// <summary>Downloads the duel log from FTP.</summary>
        /// <param name="EndPointFTP">The end point FTP.</param>
        /// <returns>Path string</returns>
        Task<string> DownloadDuelLogFromFTP(string EndPointFTP);

        /// <summary>Downloads the decks from FTP.</summary>
        /// <param name="EndPointFTP">The end point FTP.</param>
        /// <returns>Path string</returns>
        Task<string> DownloadDecksFromFTP(string EndPointFTP);
    }
}
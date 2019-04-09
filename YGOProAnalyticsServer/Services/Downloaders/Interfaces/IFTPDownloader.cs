using System.Collections.Generic;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Services.Downloaders.Interfaces
{
    /// <summary>Provides methods to download DuelLogFromFTP</summary>
    public interface IFTPDownloader
    {
        /// <summary>Downloads the duel log from FTP.</summary>
        /// <param name="EndPointFTP">The end point FTP.</param>
        /// <returns>Path string of downloaded file</returns>
        Task<string> DownloadDuelLogFromFTP(string EndPointFTP);

        /// <summary>Downloads the decks from FTP.</summary>
        /// <param name="EndPointFTP">The end point FTP.</param>
        /// <returns>Path string of downloaded file</returns>
        Task<string> DownloadDecksFromFTP(string EndPointFTP);

        /// <summary>Downloads the list of files from FTP.</summary>
        /// <param name="EndPointFTP">The end point FTP.</param>
        /// <returns>Returns list of file names of files found under the FTP Link</returns>
        List<string> DownloadListOfFilesFromFTP(string EndPointFTP);
    }
}
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Services.Downloaders.Interfaces
{
    /// <summary>
    /// Provide methods required to download banlist.
    /// </summary>
    public interface IBanlistDataDownloader
    {
        /// <summary>
        /// Downloads a lflist.conf in string format from provided URL.
        /// </summary>
        /// <param name="websiteURL">URL Address of the lflist.conf.</param>
        /// <returns>returns lflist.conf in a string format.</returns>
        Task<string> DownloadBanlistFromWebsite(string websiteURL);
    }
}
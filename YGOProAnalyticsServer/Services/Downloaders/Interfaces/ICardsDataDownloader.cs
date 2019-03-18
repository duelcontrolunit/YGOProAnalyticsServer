using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Services.Downloaders.Interfaces
{
    /// <summary>
    /// Provide methods required to download banlist.
    /// </summary>
    public interface ICardsDataDownloader
    {
        /// <summary>
        /// Downloads a JSON containing cards data in string format from provided API.
        /// </summary>
        /// <param name="EndPointURL">URL Address of the cards data API.</param>
        /// <returns>returns cards data in a string format.</returns>
        Task<string> DownloadCardsFromWebsite(string EndPointURL);
    }
}
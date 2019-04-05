using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Services.Downloaders.Interfaces
{
    /// <summary>
    /// Class which implement this is responsible for downloading list of rooms 
    /// (both open and closed) from YGOPro game server.
    /// </summary>
    public interface IYGOProServerRoomsDownloader
    {
        /// <summary>
        /// Downloads the list of rooms from YGOPro game server.
        /// </summary>
        /// <param name="EndPointURL">The end point URL.</param>
        /// <returns>List of rooms as in JSON format.</returns>
        Task<string> DownloadListOfRooms(string EndPointURL);
    }
}
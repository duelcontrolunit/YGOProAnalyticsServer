using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServer.Services.Downloaders
{
    /// <summary>
    /// Is responsible for downloading list of rooms 
    /// (both open and closed) from YGOPro game server.
    /// </summary>
    public class YGOProServerRoomsDownloader : IYGOProServerRoomsDownloader
    {
        /// <inheritdoc />
        public async Task<string> DownloadListOfRooms(string EndPointURL)
        {
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                return await client.DownloadStringTaskAsync(new Uri(EndPointURL));
            }
        }
    }
}

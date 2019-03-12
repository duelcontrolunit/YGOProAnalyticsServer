using System;
using System.Net;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServer.Services.Downloaders
{
    public class BanlistDataDownloader : IBanlistDataDownloader
    {
        /// <summary>
        /// Downloads a lflist.conf in string format from provided URL.
        /// </summary>
        /// <param name="websiteURL">URL Address of the lflist.conf.</param>
        /// <returns>returns lflist.conf in a string format.</returns>
        public async Task<string> DownloadBanlistFromWebsite(string websiteURL)
        {
            using(var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                return await client.DownloadStringTaskAsync(new Uri(websiteURL));
            }
        }
    }
}

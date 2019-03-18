using System;
using System.Net;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServer.Services.Downloaders
{
    /// <summary>
    /// You can use it to download cards data from Api.
    /// </summary>
    /// <seealso cref="YGOProAnalyticsServer.Services.Downloaders.Interfaces.ICardsDataDownloader" />
    public class CardsDataDownloader : ICardsDataDownloader
    {
        /// <summary>
        /// Downloads a JSON containing cards data in string format from provided API.
        /// </summary>
        /// <param name="EndPointURL">URL Address of the cards data API.</param>
        /// <returns>
        /// returns cards data in a string format.
        /// </returns>
        public async Task<string> DownloadCardsFromWebsite(string EndPointURL)
        {
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                return await client.DownloadStringTaskAsync(new Uri(EndPointURL));
            }
        }
    }
}

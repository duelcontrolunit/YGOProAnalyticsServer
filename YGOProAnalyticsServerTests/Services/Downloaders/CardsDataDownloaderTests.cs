using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;
using YGOProAnalyticsServer.Services.Downloaders;

namespace YGOProAnalyticsServerTests.Services.Downloaders
{
    [TestFixture]
    class CardsDataDownloaderTests
    {
        ICardsDataDownloader _downloader;

        [Test]
        public async Task DownloadBanlistFromWebsite_SourceShouldBeAvailable_WeGetDataAsStringLongerThen0()
        {
            _downloader = new CardsDataDownloader();
            string result = await _downloader.DownloadCardsFromWebsite("https://db.ygoprodeck.com/api/v3/cardinfo.php");

            Assert.NotZero(result.Count());
        }
    }
}

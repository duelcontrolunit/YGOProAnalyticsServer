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
        public async Task DownloadCardsFromWebsite_SourceShouldBeAvailable_WeGetDataAsStringLongerThen0()
        {
            _downloader = new CardsDataDownloader();
            string result = await _downloader.DownloadCardsFromWebsite("https://db.ygoprodeck.com/api/v6/cardinfo.php");

            Assert.NotZero(result.Count());
        }

        [Test]
        public void DownloadCardsFromWebsite_WrongUrlIsGiven_WeGetWebException()
        {
            _downloader = new CardsDataDownloader();

            Assert.ThrowsAsync<System.Net.WebException>(async() => await _downloader.DownloadCardsFromWebsite("Wronghttps://db.ygoprodeck.com/api/v6/cardinfo.php"));
        }
    }
}

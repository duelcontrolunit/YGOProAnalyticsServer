using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Services.Downloaders;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServerTests.Services.Downloaders
{
    [TestFixture]
    class BanlistDataDownloaderTests
    {
        IBanlistDataDownloader _downloader; 

        [Test]
        public async Task DownloadBanlistFromWebsite_SourceShouldBeAvailable_WeGetDataAsStringLongerThen0()
        {
            _downloader = new BanlistDataDownloader();
            string result = await _downloader.DownloadBanlistFromWebsite("https://raw.githubusercontent.com/szefo09/updateYGOPro2/master/lflist.conf");

            Assert.NotZero(result.Count());
        }

        [Test]
        public void DownloadCardsFromWebsite_WrongUrlIsGiven_WeGetWebException()
        {
            _downloader = new BanlistDataDownloader();

            Assert.ThrowsAsync<System.Net.WebException>(async () => await _downloader.DownloadBanlistFromWebsite("Wronghttps://db.ygoprodeck.com/api/v3/cardinfo.php"));
        }
    }
}

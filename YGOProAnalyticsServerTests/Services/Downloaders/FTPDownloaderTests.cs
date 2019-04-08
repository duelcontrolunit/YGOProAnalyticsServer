using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer;
using YGOProAnalyticsServer.Services.Downloaders;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;
using Moq;

namespace YGOProAnalyticsServerTests.Services.Downloaders
{
    [TestFixture]
    class FTPDownloaderTests
    {
        IFTPDownloader _FTPDownloader;
        Mock<IAdminConfig> _adminConfigMock;
        [SetUp]
        public void SetUp()
        {
            _adminConfigMock = new Mock<IAdminConfig>();
        }

        [Test]
        public void DownloadDuelLogFromFTP_WrongUrlIsGiven_WeGetWebException()
        {
            _FTPDownloader = new FTPDownloader(new AdminConfig());
            Assert.ThrowsAsync<System.Net.WebException>(async () => await _FTPDownloader.DownloadDuelLogFromFTP("ftp://NotExsitingWebsite.co423m"));
        }
        [Test]
        public void DownloadDecksFromFTP_WrongUrlIsGiven_WeGetWebException()
        {
            _FTPDownloader = new FTPDownloader(new AdminConfig());
            Assert.ThrowsAsync<System.Net.WebException>(async () => await _FTPDownloader.DownloadDecksFromFTP("ftp://NotExsitingWebsite.co423m"));
        }
    }
}

using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer;
using YGOProAnalyticsServer.Services.Downloaders;
using YGOProAnalyticsServer.Services.Others;

namespace YGOProAnalyticsServerTests.Services.Others
{
    [TestFixture]
    class YgoProServerStatusServiceTests
    {
        YgoProServerStatusService _serverStatus; 

        [Test]
        public async Task IsOnline()
        {
            _serverStatus = new YgoProServerStatusService(new YGOProServerRoomsDownloader());
            Assert.AreEqual(16, await _serverStatus.NumberOfPlayersInLobby("http://szefoserver.ddns.net:7211/api/getrooms?&pass="));
        }
    }
}

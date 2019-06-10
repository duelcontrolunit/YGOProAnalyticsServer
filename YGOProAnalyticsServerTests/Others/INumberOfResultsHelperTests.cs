using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer;
using YGOProAnalyticsServer.Services.Others;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServerTests.Others
{
    [TestFixture]
    class NumberOfResultsHelperTests
    {
        [TestCase(10, 10)]
        [TestCase(200, 100)]
        [TestCase(-1, 100)]
        public void GetNumberOfResultsPerPage_WeGetValidParam_ReturnsInt(
            int numberOfResultsRequestedByUser,
            int numberOfResultsServedByServer)
        {
            var configMock = new Mock<IAdminConfig>();
            configMock
                .Setup(x => x.MaxNumberOfResultsPerBrowserPage)
                .Returns(100);
            configMock
                .Setup(x => x.DefaultNumberOfResultsPerBrowserPage)
                .Returns(100);

            var helper = new NumberOfResultsHelper(configMock.Object);

            Assert.AreEqual(
                numberOfResultsServedByServer,
                helper.GetNumberOfResultsPerPage(numberOfResultsRequestedByUser));
        }
    }
}

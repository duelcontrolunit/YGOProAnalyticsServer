using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServerTests.DbModels
{
    [TestFixture]
    class BanlistTests
    {
        [Test]
        public void ReleaseDate_NameValid_DatesAreEqual()
        {
            var banlist = new Banlist(
                "2020.01 BobPatrzy",
                new List<Card>(),
                new List<Card>(),
                new List<Card>()
                );

            Assert.AreEqual(new DateTime(2020, 1, 1), banlist.ReleaseDate);
        }

        [Test]
        public void ReleaseDate_NameInvalid_ThrowsFormatException()
        {
            var banlist = new Banlist(
                "2020.01u BobPatrzy",
                new List<Card>(),
                new List<Card>(),
                new List<Card>()
                );

            Assert.Throws<FormatException>(() => { var x = banlist.ReleaseDate; });
        }
    }
}

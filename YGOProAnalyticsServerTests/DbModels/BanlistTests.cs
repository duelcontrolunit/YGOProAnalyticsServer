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
            var banlist = new Banlist("2020.01 BobPatrzy");

            Assert.AreEqual(new DateTime(2020, 1, 1), banlist.ReleaseDate);
        }

        [Test]
        public void ReleaseDate_NameInvalid_ThrowsFormatException()
        {
            var banlist = new Banlist("2020.01u BobPatrzy");

            Assert.Throws<FormatException>(() => { var x = banlist.ReleaseDate; });
        }

        [Test]
        public void Format_NameValid_WeGetExpectedFormat()
        {
            var banlist = new Banlist("2020.01 BobPatrzy");

            Assert.AreEqual("BobPatrzy", banlist.Format);
        }

        [Test]
        public void Format_NameInvalid_WeGetInvalidFormat()
        {
            var banlist = new Banlist("2020.01BobPatrzy]");

            Assert.AreNotEqual("BobPatrzy", banlist.Format);
        }
    }
}

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
            Assert.Throws<FormatException>(() => new Banlist("2020.01u BobPatrzy"));
        }

        [Test]
        public void Format_NameValid_WeGetExpectedFormat()
        {
            var banlist = new Banlist("2020.01 BobPatrzy");

            Assert.AreEqual("BobPatrzy", banlist.Format);
        }

        [Test]
        public void Format_NameInvalid_WeGetFormatException()
        {
            Assert.Throws<FormatException>(() => new Banlist("2020.01BobPatrzy]"));
        }
    }
}

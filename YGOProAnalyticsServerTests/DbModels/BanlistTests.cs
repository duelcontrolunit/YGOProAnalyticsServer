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
        public void GetReleaseDateFromName_NameValid_DatesAreEqual()
        {
            var banlist = new Banlist("2020.01 BobPatrzy", 1);

            Assert.AreEqual(new DateTime(2020, 1, 1), banlist.GetReleaseDateFromName());
        }

        [Test]
        public void Constructor_NameInvalid_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => new Banlist("2020.01u BobPatrzy", 1));
        }

        [Test]
        public void Format_NameValid_WeGetExpectedFormat()
        {
            var banlist = new Banlist("2020.01 BobPatrzy", 1);

            Assert.AreEqual("BobPatrzy", banlist.Format);
        }
    }
}

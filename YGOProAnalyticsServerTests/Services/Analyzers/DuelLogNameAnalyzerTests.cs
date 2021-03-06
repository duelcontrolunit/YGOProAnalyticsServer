﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using YGOProAnalyticsServer;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Exceptions;
using YGOProAnalyticsServer.Services.Analyzers;
using YGOProAnalyticsServer.Services.Analyzers.Interfaces;
using YGOProAnalyticsServer.Services.Converters.Interfaces;

namespace YGOProAnalyticsServerTests.Services.Analyzers
{
    [TestFixture]
    class DuelLogNameAnalyzerTests
    {
        IDuelLogNameAnalyzer _analyzer;
        Mock<IAdminConfig> _adminConfigMock;
        Mock<IDuelLogConverter> _duelLogConverter;
        YgoProAnalyticsDatabase _db;

        [SetUp]
        public void SetUp()
        {
            _db = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>());
            _db.Database.EnsureCreated();
            _adminConfigMock = new Mock<IAdminConfig>();
            _duelLogConverter = new Mock<IDuelLogConverter>();
            _analyzer = new DuelLogNameAnalyzer(_db, _adminConfigMock.Object, _duelLogConverter.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        [TestCase("S,RANDOM#39848 (Duel:1)")]
        [TestCase("S,RANDOM#97062 (Duel:1)")]
        [TestCase("LF2#2222 (Duel:1)")]
        [TestCase("LF2,NS#,1534 (Duel:1)")]
        [TestCase("M,LF2,LP8000,TM999# (Duel:2)")]
        [TestCase("Kuriboh (Duel:1)")]
        public void IsAnyBanlist_InRoomNameIsInformationAboutBanlist_ReturnsTrue(string duelLogName)
        {
            Assert.IsTrue(_analyzer.IsAnyBanlist(duelLogName));
        }

        [TestCase("NC# (Duel:1)")]
        [TestCase("NF,TM999#kirby (Duel:1)")]
        [TestCase("NF#あ (Duel:1)")]
        [TestCase("NF,M#CANELO (Duel:3)")]
        [TestCase("AI#62257")]
        [TestCase("AI#Salaman,20088")]
        [TestCase("LF2,NC#2222 (Duel:1)")]
        public void IsAnyBanlist_NoInformationOrInformationAboutNoBanlist_ReturnsFalse(string duelLogName)
        {
            Assert.IsFalse(_analyzer.IsAnyBanlist(duelLogName));
        }

        [TestCase("AI#62257")]
        [TestCase("AI#Salaman,20088")]
        [TestCase("AINCRAD#23144")]
        [TestCase("AIncrad#23145")]
        [TestCase("AI,T#854")]
        [TestCase("AI,NF,NS,NC,T#855")]
        public void IsDuelVersusAI_IsDuelVsAI_ReturnsTrue(string duelLogName)
        {
            Assert.IsTrue(_analyzer.IsDuelVersusAI(duelLogName));
        }

        [TestCase("AI,NF,NS,NC,T#Salaman,855")]
        [TestCase("AINCRAD#Salaman,20088")]
        [TestCase("AIncrad#Salaman,20088")]
        [TestCase("S,RANDOM#39848 (Duel:1)")]
        [TestCase("S,RANDOM#97062 (Duel:1)")]
        [TestCase("LF2#2222 (Duel:1)")]
        [TestCase("LF2,NS#,1534 (Duel:1)")]
        [TestCase("M,LF2,LP8000,TM999# (Duel:2)")]
        [TestCase("Kuriboh (Duel:1)")]
        [TestCase("NC# (Duel:1)")]
        [TestCase("NF,TM999#kirby (Duel:1)")]
        [TestCase("NF#あ (Duel:1)")]
        [TestCase("NF,M#CANELO (Duel:3)")]
        [TestCase("TO,M,R (Duel:1)")]
        [TestCase("M#dragon (Duel:1)")]
        [TestCase("TO,OT,M#Alex (Duel:2)")]
        [TestCase("T#cudomarcus (Duel:1)")]
        [TestCase("5 (Duel:1)")]
        [TestCase("M#01 (Duel:1)")]
        public void IsDuelVersusAI_IsNotDuelVsAI_ReturnsFalse(string duelLogName)
        {
            Assert.IsFalse(_analyzer.IsDuelVersusAI(duelLogName));
        }

        [TestCase("NC# (Duel:1)")]
        [TestCase("T,NC# (Duel:1)")]
        [TestCase("T,NC,NS# (Duel:1)")]
        public void IsNoDeckCheckEnabled_IsEnabled_ReturnTrue(string duelLogName)
        {
            Assert.IsTrue(_analyzer.IsNoDeckCheckEnabled(duelLogName));
        }

        [TestCase("S,RANDOM#39848 (Duel:1)")]
        [TestCase("S,RANDOM#97062 (Duel:1)")]
        [TestCase("LF2#2222 (Duel:1)")]
        [TestCase("LF2,NS#,1534 (Duel:1)")]
        [TestCase("M,LF2,LP8000,TM999# (Duel:2)")]
        [TestCase("Kuriboh (Duel:1)")]
        [TestCase("NF,TM999#kirby (Duel:1)")]
        [TestCase("NF#あ (Duel:1)")]
        [TestCase("NF,M#CANELO (Duel:3)")]
        public void IsNoDeckCheckEnabled_IsDisabled_ReturnFalse(string duelLogName)
        {
            Assert.IsFalse(_analyzer.IsNoDeckCheckEnabled(duelLogName));
        }

        [TestCase("S,RANDOM#39848 (Duel:1)")]
        [TestCase("S,RANDOM#97062 (Duel:1)")]
        [TestCase("Kuriboh (Duel:1)")]
        [TestCase("OT,TO,S#LosMorros (Duel:1)")]
        [TestCase("OT,TO,M#Piolo (Duel:1)")]
        [TestCase("T,RANDOM#36472 (Duel:1)")]
        [TestCase("M#dragon (Duel:2)")]
        [TestCase("2222 (Duel:1)")]
        [TestCase("LP2222 (Duel:1)")]
        [TestCase("LP2222,PR,TM999,ST44# (Duel:1)")]
        public void IsDefaultanlist_IsInformationAboutDefaultBanlist_ReturnTrue(string duelLogName)
        {
            Assert.IsTrue(_analyzer.IsDefaultBanlist(duelLogName));
        }

        [TestCase("LF2#2222 (Duel:1)")]
        [TestCase("LF2,NS#,1534 (Duel:1)")]
        [TestCase("M,LF2,LP8000,TM999# (Duel:2)")]
        [TestCase("NF,TM999#kirby (Duel:1)")]
        [TestCase("NF#あ (Duel:1)")]
        [TestCase("NF,M#CANELO (Duel:3)")]
        [TestCase("NC# (Duel:1)")]
        [TestCase("T,NC# (Duel:1)")]
        [TestCase("AI#62257")]
        [TestCase("AI#Salaman,20088")]
        public void IsDefaultBanlist_IsNoInformationAboutDefaultBanlist_ReturnFalse(string duelLogName)
        {
            Assert.IsFalse(_analyzer.IsDefaultBanlist(duelLogName));
        }

        [TestCase("M,LF2,NS,LP8000,TM999# (Duel:2)")]
        [TestCase("NS,M,LF2,NS,LP8000,TM999# (Duel:2)")]
        [TestCase("NS,M,LF2,NS,LP8000,TM999,NS# (Duel:2)")]
        [TestCase("AI,NS#62257")]
        [TestCase("T,NC,NS# (Duel:1)")]
        [TestCase("LF2,NS#,1534 (Duel:1)")]
        [TestCase("OT,TO,S,NS#LosMorros (Duel:1)")]
        [TestCase("M,NS#dragon (Duel:2)")]
        public void IsNoDeckShuffleEnabled_IsEnabled_ReturnsTrue(string duelLogName)
        {
            Assert.IsTrue(_analyzer.IsNoDeckShuffleEnabled(duelLogName));
        }

        [TestCase("M,LF2,LP8000,TM999# (Duel:2)")]
        [TestCase("M#dragon (Duel:2)")]
        [TestCase("OT,TO,S#LosMorros (Duel:1)")]
        [TestCase("LF2#,1534 (Duel:1)")]
        [TestCase("LF2#2222 (Duel:1)")]
        [TestCase("M,LF2,LP8000,TM999# (Duel:2)")]
        [TestCase("NF,TM999#kirby (Duel:1)")]
        [TestCase("NF#あ (Duel:1)")]
        [TestCase("NF,M#CANELO (Duel:3)")]
        [TestCase("NC# (Duel:1)")]
        [TestCase("AI#62257")]
        [TestCase("AI#Salaman,20088")]
        public void IsNoDeckShuffleEnabled_IsDisabled_ReturnsFalse(string duelLogName)
        {
            Assert.IsFalse(_analyzer.IsNoDeckShuffleEnabled(duelLogName));
        }

        [TestCase("S,RANDOM#39848 (Duel:1)", "2019-03-19 16-22-26")]
        [TestCase("S,RANDOM#97062 (Duel:1)", "2019-03-19 16-22-26")]
        [TestCase("LF2#2222 (Duel:1)", "2019-03-19 16-22-26")]
        [TestCase("LF2,NS#,1534 (Duel:1)", "2019-03-19 16-22-26")]
        [TestCase("M,LF2,LP8000,TM999# (Duel:2)", "2019-03-19 16-22-26")]
        [TestCase("Kuriboh (Duel:1)", "2019-03-19 16-22-26")]
        [TestCase("NF,TM999#kirby (Duel:1)", "2019-03-19 16-22-26")]
        [TestCase("NF#あ (Duel:1)", "2019-03-19 16-22-26")]
        [TestCase("NF,M#CANELO (Duel:3)", "2019-03-19 16-22-26")]
        [TestCase("LF2#2222 (Duel:1)", "2019-03-19 16-22-26")]
        public void GetBanlist_DuelLogNameDoNotContainInfoAboutAnyBanlistOrBanlistIsNotKnown_WeGetUnnownBanlistException(
            string duelLogName,
            string duelLogDate)
        {
            using (var db = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                db.Database.EnsureCreated();
                _adminConfigMock = new Mock<IAdminConfig>();
                _duelLogConverter = new Mock<IDuelLogConverter>();
                _analyzer = new DuelLogNameAnalyzer(db, _adminConfigMock.Object, _duelLogConverter.Object);
                Assert.Throws<UnknownBanlistException>(() => _analyzer.GetBanlist(
                    duelLogName,
                    _convertDuelLogTimeToDateTime(duelLogDate)));
            }
        }

        [TestCase("S,RANDOM#39848 (Duel:1)", "2019-03-19 16-22-26")]
        [TestCase("S,RANDOM#97062 (Duel:1)", "2019-03-19 16-22-26")]
        [TestCase("Kuriboh (Duel:1)", "2019-03-19 16-22-26")]
        public void GetBanlist_DuelLogDoNotContainInfoAboutAnyBanlistOrAnyNoDeckLikeNC_WeGetDefaultBanlist(
            string duelLogName,
            string duelLogDate)
        {
            using (var db = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                db.Database.EnsureCreated();
                var adminConfigMock = new Mock<IAdminConfig>();
                adminConfigMock
                    .Setup(x => x.DefaultBanlistNumber)
                    .Returns(1);
                var duelLogConverter = new Mock<IDuelLogConverter>();
                db.Banlists.Add(new Banlist("2019.03 TCG", 1));
                db.SaveChanges();
                _analyzer = new DuelLogNameAnalyzer(db, adminConfigMock.Object, duelLogConverter.Object);

                var banlist = _analyzer.GetBanlist(duelLogName, _convertDuelLogTimeToDateTime(duelLogDate));

                Assert.AreEqual("2019.03 TCG", banlist.Name);
            }
        }

        [TestCase("LF2#2222 (Duel:1)", "2019-03-19 16-22-26")]
        [TestCase("LF1#2222 (Duel:1)", "2019-03-19 16-22-26")]
        [TestCase("LF2,NS#,1534 (Duel:1)", "2019-03-19 16-22-26")]
        public void GetBanlist_DuelLogNameContainInfoAboutBanlistButBanlistIsNotKnown_WeGetUnnownBanlistException(
           string duelLogName,
           string duelLogDate)
        {
            using (var db = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                db.Database.EnsureCreated();
                _analyzer = new DuelLogNameAnalyzer(db, _adminConfigMock.Object, _duelLogConverter.Object);

                Assert.Throws<UnknownBanlistException>(() => _analyzer.GetBanlist(
                    duelLogName,
                    _convertDuelLogTimeToDateTime(duelLogDate)));
            }
        }

        [TestCase("LF2#2222 (Duel:1)", "2019-03-19 16-22-26", "2019.03 OCG")]
        [TestCase("LF1#2222 (Duel:1)", "2019-03-19 16-22-26", "2019.03 TCG")]
        public void GetBanlist_DuelLogNameContainInfoAboutBanlistAndIsKnownMoreThanOneBanlist_WeGetRequestedBanlist(
           string duelLogName,
           string duelLogDate,
           string banlistName)
        {
            using (var db = new YgoProAnalyticsDatabase(_getOptionsForSqlInMemoryTesting<YgoProAnalyticsDatabase>()))
            {
                db.Database.EnsureCreated();
                var duelLogConverterMock = new Mock<IDuelLogConverter>();
                var logDateTime = DateTime.Parse("2019-03-19 16:22:26");
                duelLogConverterMock
                    .Setup(x => x.ConvertDuelLogTimeToDateTime(duelLogDate))
                    .Returns(logDateTime);
                db.Banlists.Add(new Banlist("2019.03 TCG", 1));
                db.Banlists.Add(new Banlist("2019.03 OCG", 1));
                db.SaveChanges();
                _analyzer = new DuelLogNameAnalyzer(db, _adminConfigMock.Object, duelLogConverterMock.Object);

                var banlist = _analyzer.GetBanlist(duelLogName, _convertDuelLogTimeToDateTime(duelLogDate));

                Assert.AreEqual(banlistName, banlist.Name);
            }
        }

        private DbContextOptions<T> _getOptionsForSqlInMemoryTesting<T>() where T : DbContext
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            return new DbContextOptionsBuilder<T>()
                .UseSqlite(connection)
                .ConfigureWarnings(x => x.Ignore(RelationalEventId.QueryClientEvaluationWarning))
                .Options;
        }

        private DateTime _convertDuelLogTimeToDateTime(string duelLogTime)
        {
            if (!Regex.IsMatch(duelLogTime, @"\d{4}-\d{2}-\d{2} \d{2}-\d{2}-\d{2}"))
            {
                throw new FormatException("Wrong format.");
            }

            string dateOfTheEndOfTheDuel = duelLogTime.Substring(0, duelLogTime.IndexOf(' '));
            var date = dateOfTheEndOfTheDuel.Split('-');

            string timeOfTheEndOfTheDuel = duelLogTime.Substring(duelLogTime.IndexOf(' '));
            var time = timeOfTheEndOfTheDuel.Split('-');

            return DateTime
                .Parse(
                    $"{date[0]}/{date[1]}/{date[2]} {time[0]}:{time[1]}:{time[2]}"
                    , CultureInfo.InvariantCulture);
        }
    }
}

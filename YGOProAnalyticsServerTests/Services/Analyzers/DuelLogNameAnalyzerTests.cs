using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.Services.Analyzers;
using YGOProAnalyticsServer.Services.Analyzers.Interfaces;

namespace YGOProAnalyticsServerTests.Services.Analyzers
{
    [TestFixture]
    class DuelLogNameAnalyzerTests
    {
        IDuelLogNameAnalyzer _analyzer;
        Mock<YgoProAnalyticsDatabase> _dbMock;
        Mock<IAdminConfig> _adminConfigMock;
        DbContextOptions<YgoProAnalyticsDatabase> _dbOptions = new DbContextOptionsBuilder<YgoProAnalyticsDatabase>().Options;

        [SetUp]
        public void SetUp()
        {
            _dbMock = new Mock<YgoProAnalyticsDatabase>(_dbOptions);
            _adminConfigMock = new Mock<IAdminConfig>();
            _analyzer = new DuelLogNameAnalyzer(_dbMock.Object, _adminConfigMock.Object);
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
        public void IsAnyBanlist_NoInformationOrInformationAboutNoBanlist_ReturnsFalse(string duelLogName)
        {
            Assert.IsFalse(_analyzer.IsAnyBanlist(duelLogName));
        }

        [TestCase("AI#62257")]
        [TestCase("AI#Salaman,20088")]
        public void IsDuelVersusAI_IsDuelVsAI_ReturnsTrue(string duelLogName)
        {
            Assert.IsTrue(_analyzer.IsDuelVersusAI(duelLogName));
        }

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
        public void IsDuelVersusAI_IsNotDuelVsAI_ReturnsFalse(string duelLogName)
        {
            Assert.IsFalse(_analyzer.IsDuelVersusAI(duelLogName));
        }

        [TestCase("NC# (Duel:1)")]
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
        public void IsDefaultBanlist_IsInformationAboutDefaultBanlist_ReturnTrue(string duelLogName)
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
        [TestCase("AI#62257")]
        [TestCase("AI#Salaman,20088")]
        public void IsDefaultBanlist_IsNoInformationAboutDefaultBanlist_ReturnFalse(string duelLogName)
        {
            Assert.IsFalse(_analyzer.IsDefaultBanlist(duelLogName));
        }

        [TestCase("LF2,NS#,1534 (Duel:1)")]
        public void IsNoDeckShuffleEnabled_IsEnabled_ReturnsTrue(string duelLogName)
        {
            Assert.IsTrue(_analyzer.IsNoDeckShuffleEnabled(duelLogName));
        }

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
    }
}

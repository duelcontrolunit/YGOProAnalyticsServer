using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YGOProAnalyticsServer.Exceptions;
using YGOProAnalyticsServer.Services.Converters;
using YGOProAnalyticsServer.Services.Converters.Interfaces;

namespace YGOProAnalyticsServerTests.Services.Converters
{
    [TestFixture]
    class DuelLogConverterTests
    {
        IDuelLogConverter _converter;

        [SetUp]
        public void SetUp()
        {
            _converter = new DuelLogConverter();
        }

        [Test]
        public void Convert_WeHaveValidDuelLogJSON_WeGetTwoDuelLogs()
        {
            var duelLogs = _converter.Convert(_getValidDuelLogJSON());
            Assert.AreEqual(2, duelLogs.Count());
        }

        [Test]
        public void Convert_WePassNullAsParameter_WeGetArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _converter.Convert(null));
        }

        [Test]
        public void Convert_WeHaveValidDuelLogJson_DeckFileNameWhichWonIsInValidFormat()
        {
            var duelLogs = _converter.Convert(_getValidDuelLogJSON());

            Assert.AreEqual("2019-06-02 16-42-28 10346 0 CMK", duelLogs[0].DecksWhichWonFileNames[0]);
            Assert.AreEqual("2019-06-02 16-41-18 10180 1 Soulindo", duelLogs[1].DecksWhichWonFileNames[0]);
        }

        [Test]
        public void Convert_WeHaveValidDuelLogJson_DeckFileNameWhichLostIsInValidFormat()
        {
            var duelLogs = _converter.Convert(_getValidDuelLogJSON());

            Assert.AreEqual("2019-06-02 16-42-28 10346 1 TheTricky92", duelLogs[0].DecksWhichLostFileNames[0]);
            Assert.AreEqual("2019-06-02 16-41-18 10180 0 Ninja47", duelLogs[1].DecksWhichLostFileNames[0]);
        }

        [TestCase("2019-03-19 16-22-26", 2019, 3, 19, 16, 22, 26)]
        [TestCase("2019-03-19 16-22-00", 2019, 3, 19, 16, 22, 00)]
        public void ConvertDuelLogTimeToDateTime_WePassValidDuelLogTime_WeGetValidDateTime(
            string duelLogDateTime,
            int validYear,
            int validMonth,
            int validDay,
            int validHour,
            int validMinute,
            int validSecond)
        {
            DateTime convertedDateTime = _converter.ConvertDuelLogTimeToDateTime(duelLogDateTime);

            Assert.AreEqual(validYear, convertedDateTime.Year);
            Assert.AreEqual(validMonth, convertedDateTime.Month);
            Assert.AreEqual(validDay, convertedDateTime.Day);

            Assert.AreEqual(validHour, convertedDateTime.Hour);
            Assert.AreEqual(validMinute, convertedDateTime.Minute);
            Assert.AreEqual(validSecond, convertedDateTime.Second);
        }

        [TestCase("")]
        [TestCase("2019/03/19 16:22:26")]
        [TestCase("2019-03-19")]
        [TestCase("16:22:26")]
        [TestCase("19/03/2019 16:22:00")]
        public void ConvertDuelLogTimeToDateTime_WePassInvalidDuelLogTime_WeGetFormatException(string dateTimeInInvalidFormat)
        {
            Assert.Throws<FormatException>(() => _converter.ConvertDuelLogTimeToDateTime(dateTimeInInvalidFormat));
        }

        private string _getValidDuelLogJSON()
        {
            return "{'file':'./ config / duel_log.json','duel_log':[{'starttime':'2019-06-02 16-42-28','endtime':'2019-06-02 16-51-43','name':'S,RANDOM#81209 (Duel:1)','roomid':'10346','cloud_replay_id':'R#70319758','replay_filename':'2019-06-02 16-42-28 CMK VS TheTricky92.yrp','roommode':0,'players':[{'name':'CMK (Score:1 LP:7000 Cards:4)','winner':true,'deckname':'2019-06-02 16-42-28 10346 0 CMK'},{'name':'TheTricky92 (Score:0 LP:0 Cards:2)','winner':false,'deckname':'2019-06-02 16-42-28 10346 1 TheTricky92'}]},{'starttime':'2019-06-02 16-41-18','endtime':'2019-06-02 16-51-06','name':'S,RANDOM#78799 (Duel:1)','roomid':'10180','cloud_replay_id':'R#99569846','replay_filename':'2019-06-02 16-41-18 Ninja47 VS Soulindo.yrp','roommode':0,'players':[{'name':'Ninja47 (Score:0 LP:7200 Cards:2)','winner':false,'deckname':'2019-06-02 16-41-18 10180 0 Ninja47'},{'name':'Soulindo (Score:1 LP:9000 Cards:5)','winner':true,'deckname':'2019-06-02 16-41-18 10180 1 Soulindo'}]}]";
        }
    }
}

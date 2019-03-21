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

            Assert.AreEqual("2019-03-19 16-22-26 15374 1 Ghost Player.ydk", duelLogs[0].DecksWhichWonFileNames[0]);
            Assert.AreEqual("2019-03-19 16-22-00 16028 1 Art.ydk", duelLogs[1].DecksWhichWonFileNames[0]);
        }

        [Test]
        public void Convert_WeHaveValidDuelLogJson_DeckFileNameWhichLostIsInValidFormat()
        {
            var duelLogs = _converter.Convert(_getValidDuelLogJSON());

            Assert.AreEqual("2019-03-19 16-22-26 15374 0 abdulaziz.ydk", duelLogs[0].DecksWhichLostFileNames[0]);
            Assert.AreEqual("2019-03-19 16-22-00 16028 0 GutsuVenom.ydk", duelLogs[1].DecksWhichLostFileNames[0]);
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
            return "{\r\n  \"file\": \"NotImportantData\",\r\n  \"duel_log\": [\r\n    {\r\n      \"time\": \"2019-03-19 16-22-26\",\r\n      \"name\": \"S,RANDOM#39848 (Duel:1)\",\r\n      \"roomid\": \"15374\",\r\n      \"cloud_replay_id\": \"R#51303942\",\r\n      \"replay_filename\": \"2019-03-19 16-22-26 abdulaziz VS Ghost Player.yrp\",\r\n      \"roommode\": 0,\r\n      \"players\": [\r\n        {\r\n          \"name\": \"abdulaziz (Score:0 LP:6100 Cards:3)\",\r\n          \"winner\": false\r\n        },\r\n        {\r\n          \"name\": \"Ghost Player (Score:1 LP:8000 Cards:4)\",\r\n          \"winner\": true\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"time\": \"2019-03-19 16-22-00\",\r\n      \"name\": \"S,RANDOM#97062 (Duel:1)\",\r\n      \"roomid\": \"16028\",\r\n      \"cloud_replay_id\": \"R#33858198\",\r\n      \"replay_filename\": \"2019-03-19 16-22-00 Art VS GutsuVenom.yrp\",\r\n      \"roommode\": 0,\r\n      \"players\": [\r\n        {\r\n          \"name\": \"Art (Score:1 LP:8000 Cards:8)\",\r\n          \"winner\": true\r\n        },\r\n        {\r\n          \"name\": \"GutsuVenom (Score:0 LP:8000 Cards:5)\",\r\n          \"winner\": false\r\n        }\r\n      ]\r\n    },]}";
        }
    }
}

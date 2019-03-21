using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer.Services.Converters;

namespace YGOProAnalyticsServerTests.Services.Converters
{
    [TestFixture]
    class DuelLogConverterTests
    {
        DuelLogConverter _converter;

        [Test]
        public void Convert()
        {
            _converter = new DuelLogConverter();
            _converter.Convert(_getValidDuelLogJSON());
        }

        private string _getValidDuelLogJSON()
        {
            return "{\r\n  \"file\": \"NotImportantData\",\r\n  \"duel_log\": [\r\n    {\r\n      \"time\": \"2019-03-19 16-22-26\",\r\n      \"name\": \"S,RANDOM#39848 (Duel:1)\",\r\n      \"roomid\": \"15374\",\r\n      \"cloud_replay_id\": \"R#51303942\",\r\n      \"replay_filename\": \"2019-03-19 16-22-26 abdulaziz VS Ghost Player.yrp\",\r\n      \"roommode\": 0,\r\n      \"players\": [\r\n        {\r\n          \"name\": \"abdulaziz (IP: 78.95.41.82) (Score:0 LP:6100 Cards:3)\",\r\n          \"winner\": false\r\n        },\r\n        {\r\n          \"name\": \"Ghost Player (IP: 187.19.255.204) (Score:1 LP:8000 Cards:4)\",\r\n          \"winner\": true\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"time\": \"2019-03-19 16-22-00\",\r\n      \"name\": \"S,RANDOM#97062 (Duel:1)\",\r\n      \"roomid\": \"16028\",\r\n      \"cloud_replay_id\": \"R#33858198\",\r\n      \"replay_filename\": \"2019-03-19 16-22-00 Art VS GutsuVenom.yrp\",\r\n      \"roommode\": 0,\r\n      \"players\": [\r\n        {\r\n          \"name\": \"Art (IP: 187.237.25.68) (Score:1 LP:8000 Cards:8)\",\r\n          \"winner\": true\r\n        },\r\n        {\r\n          \"name\": \"GutsuVenom (IP: 152.171.209.225) (Score:0 LP:8000 Cards:5)\",\r\n          \"winner\": false\r\n        }\r\n      ]\r\n    },]}";
        }
    }
}

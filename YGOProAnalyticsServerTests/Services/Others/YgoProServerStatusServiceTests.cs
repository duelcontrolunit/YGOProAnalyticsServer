using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer;
using YGOProAnalyticsServer.Services.Downloaders;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;
using YGOProAnalyticsServer.Services.Others;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServerTests.Services.Others
{
    [TestFixture]
    class YgoProServerStatusServiceTests
    {
        IYgoProServerStatusService _serverStatus;
        Mock<IYGOProServerRoomsDownloader> _roomsDownloaderMock;

        [Test(Description = "Integration test")]
        public async Task IsOnlineBasedOnListOfRooms_ServerShouldOnline_ReturnsTrue()
        {
            _serverStatus = new YgoProServerStatusService(new YGOProServerRoomsDownloader());
            Assert.AreEqual(true, await _serverStatus.IsOnlineBasedOnListOfRooms("http://szefoserver.ddns.net:7211/api/getrooms?&pass="));
        }

        [SetUp]
        public void SetUp()
        {
            _roomsDownloaderMock = new Mock<IYGOProServerRoomsDownloader>();
            _roomsDownloaderMock
                .Setup(x => x.DownloadListOfRooms(It.IsAny<string>()))
                .ReturnsAsync(_getValidRoomList());
            _roomsDownloaderMock
                .Setup(x => x.DownloadListOfRooms("http://ServerIsOffline.com"))
                .Throws(new WebException());

            _serverStatus = new YgoProServerStatusService(_roomsDownloaderMock.Object);
        }

        [Test]
        public async Task IsOnlineBasedOnListOfRooms_ServerIsOnline_ReturnsTrue()
        {
            bool result = await _serverStatus
                .IsOnlineBasedOnListOfRooms("http://ServerIsOnline.com");

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsOnlineBasedOnListOfRooms_ServerIsOffline_ReturnsTrue()
        {
            bool result = await _serverStatus
                .IsOnlineBasedOnListOfRooms("http://ServerIsOffline.com");

            Assert.IsFalse(result);
        }

        [Test]
        public async Task NumberOfPlayersWhichPlayNow_WeGetDataFromServer_ReturnsNumberOfPlayersWhichPlayNow()
        {
            int result = await _serverStatus
                .NumberOfPlayersWhichPlayNow("http://ServerIsOnline.com");

            Assert.AreEqual(34, result);
        }

        [Test]
        public async Task NumberOfPlayersInLobby_WeGetDataFromServer_ReturnsNumberOfPlayersInLobby()
        {
            int result = await _serverStatus
                .NumberOfPlayersInLobby("http://ServerIsOnline.com");

            Assert.AreEqual(3, result);
        }

        [Test]
        public async Task NumberOfPlayersInAllRooms_WeGetDataFromServer_ReturnsNumberOfPlayers()
        {
            int result = await _serverStatus
                .NumberOfPlayersInAllRooms("http://ServerIsOnline.com");

            Assert.AreEqual(37, result);
        }

        [Test]
        public async Task NumberOfOpenRooms_WeGetDataFromServer_ReturnsNumberOfRooms()
        {
            int result = await _serverStatus
                .NumberOfOpenRooms("http://ServerIsOnline.com");

            Assert.AreEqual(2, result);
        }

        [Test]
        public async Task NumberOfClosedRooms_WeGetDataFromServer_ReturnsNumberOfRooms()
        {
            int result = await _serverStatus
                .NumberOfClosedRooms("http://ServerIsOnline.com");

            Assert.AreEqual(17, result);
        }

        [Test]
        public async Task NumberOfAllRooms_WeGetDataFromServer_ReturnsNumberOfRooms()
        {
            int result = await _serverStatus
                .NumberOfAllRooms("http://ServerIsOnline.com");

            Assert.AreEqual(19, result);
        }
        
        public string _getValidRoomList()
        {
            return "{\r\n  \"rooms\": [\r\n    {\r\n      \"roomid\": \"16083\",\r\n      \"roomname\": \"AI#29875\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"YGOProES Users\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 1400,\r\n            \"cards\": 1\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Toadally\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 1500,\r\n            \"cards\": 4\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:28\"\r\n    },\r\n    {\r\n      \"roomid\": \"16274\",\r\n      \"roomname\": \"AI#63290\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"YGOPro2 User\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 3\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Blue-Eyes\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 3\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:5\"\r\n    },\r\n    {\r\n      \"roomid\": \"16855\",\r\n      \"roomname\": \"AI#90719\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Pikashi\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 6000,\r\n            \"cards\": 4\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Horus\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 5600,\r\n            \"cards\": 2\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:4\"\r\n    },\r\n    {\r\n      \"roomid\": \"16914\",\r\n      \"roomname\": \"AI#90441\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"YGOPro2 User\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 6800,\r\n            \"cards\": 10\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Horus\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 3\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:5\"\r\n    },\r\n    {\r\n      \"roomid\": \"16993\",\r\n      \"roomname\": \"AI#29061\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"YGOPro2 User\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 5500,\r\n            \"cards\": 2\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Salaman\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 2\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:4\"\r\n    },\r\n    {\r\n      \"roomid\": \"17081\",\r\n      \"roomname\": \"AI#23823\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"YGOPro2 User\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 6\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Blackwing\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 800,\r\n            \"cards\": 5\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:2\"\r\n    },\r\n    {\r\n      \"roomid\": \"17140\",\r\n      \"roomname\": \"S,RANDOM#19501\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"hin\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 4\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Drake\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 5\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:1\"\r\n    },\r\n    {\r\n      \"roomid\": \"17225\",\r\n      \"roomname\": \"S,RANDOM#22373\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"YGOProES Users\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 6\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Shi\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 4\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:2\"\r\n    },\r\n    {\r\n      \"roomid\": \"17242\",\r\n      \"roomname\": \"AI#81332\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"YGOPro2 User\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 6400,\r\n            \"cards\": 6\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"CyberDragon\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 5900,\r\n            \"cards\": 4\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:6\"\r\n    },\r\n    {\r\n      \"roomid\": \"17254\",\r\n      \"roomname\": \"AI#36659\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Ghost Player\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 7100,\r\n            \"cards\": 6\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"SkyStriker\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 2\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:3\"\r\n    },\r\n    {\r\n      \"roomid\": \"17315\",\r\n      \"roomname\": \"bos\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Headder\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 4500,\r\n            \"cards\": 6\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Stein\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 6\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:7\"\r\n    },\r\n    {\r\n      \"roomid\": \"17474\",\r\n      \"roomname\": \"AI#705\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Lazyoliver101\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 5400,\r\n            \"cards\": 2\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Zexal\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 6500,\r\n            \"cards\": 0\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:3\"\r\n    },\r\n    {\r\n      \"roomid\": \"17481\",\r\n      \"roomname\": \"AI#65355\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"YGOPro2 User\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 5\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Frog\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 5\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:1\"\r\n    },\r\n    {\r\n      \"roomid\": \"17530\",\r\n      \"roomname\": \"AI#68103\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"YGOPro2 User\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 5\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"RankV\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 7700,\r\n            \"cards\": 6\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:5\"\r\n    },\r\n    {\r\n      \"roomid\": \"17587\",\r\n      \"roomname\": \"AI#26691\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"YGOPro2 User\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 7\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Blue-Eyes\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 6\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:2\"\r\n    },\r\n    {\r\n      \"roomid\": \"17691\",\r\n      \"roomname\": \"S,RANDOM#98173\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Capital Bra\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 5\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"YGOProES Users\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 5\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:1\"\r\n    },\r\n    {\r\n      \"roomid\": \"17713\",\r\n      \"roomname\": \"AI#50786\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Ridd\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 5\r\n          },\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Zoodiac\",\r\n          \"ip\": null,\r\n          \"status\": {\r\n            \"score\": 0,\r\n            \"lp\": 8000,\r\n            \"cards\": 5\r\n          },\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"Duel:1 Turn:1\"\r\n    },\r\n    {\r\n      \"roomid\": \"17756\",\r\n      \"roomname\": \"M#a\",\r\n      \"roommode\": 1,\r\n      \"needpass\": \"true\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"LoliIsJustice\",\r\n          \"ip\": null,\r\n          \"status\": null,\r\n          \"pos\": 0\r\n        }\r\n      ],\r\n      \"istart\": \"wait\"\r\n    },\r\n    {\r\n      \"roomid\": \"17763\",\r\n      \"roomname\": \"AI#89372\",\r\n      \"roommode\": 0,\r\n      \"needpass\": \"false\",\r\n      \"users\": [\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"YGOPro2 User\",\r\n          \"ip\": null,\r\n          \"status\": null,\r\n          \"pos\": 0\r\n        },\r\n        {\r\n          \"id\": \"-1\",\r\n          \"name\": \"Qliphort\",\r\n          \"ip\": null,\r\n          \"status\": null,\r\n          \"pos\": 1\r\n        }\r\n      ],\r\n      \"istart\": \"wait\"\r\n    }\r\n  ]\r\n}";
        }
    }
}

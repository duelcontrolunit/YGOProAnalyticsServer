using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Services.Others
{
    /// <summary>
    /// Provide informations about server status.
    /// </summary>
    /// <seealso cref="YGOProAnalyticsServer.Services.Others.Interfaces.IYgoProServerStatusService" />
    public class YgoProServerStatusService : IYgoProServerStatusService
    {
        readonly IYGOProServerRoomsDownloader _roomsDownloader;

        /// <summary>
        /// Initializes a new instance of the <see cref="YgoProServerStatusService"/> class.
        /// </summary>
        /// <param name="roomsDownloader">The rooms json downloader.</param>
        /// <exception cref="ArgumentNullException">roomsDownloader</exception>
        public YgoProServerStatusService(
            IYGOProServerRoomsDownloader roomsDownloader)
        {
            _roomsDownloader = roomsDownloader ?? throw new ArgumentNullException(nameof(roomsDownloader));
        }

        /// <inheritdoc />
        public async Task<bool> IsOnlineBasedOnListOfRooms(string url)
        {
            try
            {
                await _roomsDownloader.DownloadListOfRooms(url);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<int> NumberOfPlayersWhichPlayNow(string url)
        {
            return await _numberOfPlayers(url, (room) => {
                return _isDuelStarted(room);
            });
        }

        /// <inheritdoc />
        public async Task<int> NumberOfPlayersInLobby(string url)
        {
            return await _numberOfPlayers(url, (room) => {
                return !_isDuelStarted(room);
            });
        }

        /// <inheritdoc />
        public async Task<int> NumberOfPlayersInEntireGame(string url)
        {
            return await _numberOfPlayers(url, (room) => {
                return true;
            });
        }

        /// <inheritdoc />
        public async Task<int> NumberOfOpenRooms(string url)
        {
            return await _numberOfRooms(url, (room) => {
                return !_isDuelStarted(room);
            });
        }

        /// <inheritdoc />
        public async Task<int> NumberOfClosedRooms(string url)
        {
            return await _numberOfRooms(url, (room) => {
                return _isDuelStarted(room);
            });
        }

        /// <inheritdoc />
        public async Task<int> NumberOfRooms(string url)
        {
            return await _numberOfRooms(url, (room) => {
                return true;
            });
        }

        private async Task<int> _numberOfRooms(string url, Func<JObject, bool> roomFilterCondition)
        {
            var listOfRooms = await _getListOfRooms(url);
            int numberOfRooms = 0;
            foreach (JObject room in listOfRooms)
            {
                if (roomFilterCondition(room))
                {
                    numberOfRooms++;
                }
            }

            return numberOfRooms;
        }

        private async Task<int> _numberOfPlayers(string url, Func<JObject, bool> roomFilterCondition)
        {
            var listOfRooms = await _getListOfRooms(url);
            int numberOfPlayers = 0;
            foreach (JObject room in listOfRooms)
            {
                if (roomFilterCondition(room))
                {
                    numberOfPlayers += _getNumberOfPlayers(room);
                }
            }

            return numberOfPlayers;
        }

        private async Task<IEnumerable<JObject>> _getListOfRooms(string url)
        {
            string listOfRoomsJson = await _roomsDownloader.DownloadListOfRooms(url);
            return JsonConvert
               .DeserializeObject<JObject>(listOfRoomsJson)
               .First
               .First
               .Children<JObject>();
        }

        private bool _isDuelStarted(JObject room)
        {
            return room.Value<string>("istart") != "wait"; 
        }

        private int _getNumberOfPlayers(JObject room)
        {
            var users = room.GetValue("users").Children<JObject>();

            return users.Count();
        }
    }
}

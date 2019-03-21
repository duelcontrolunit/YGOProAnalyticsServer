using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Models;

namespace YGOProAnalyticsServer.Services.Converters
{
    /// <summary>
    /// It convert duel log JSON into collection of duel logs.
    /// </summary>
    public class DuelLogConverter
    {
        /// <summary>
        /// Converts the specified duel log json to list of <see cref="DuelLog"/>s.
        /// </summary>
        /// <param name="duelLogJson">The duel log json.</param>
        /// <returns>List of <see cref="DuelLog"/>s</returns>
        public List<DuelLog> Convert(string duelLogJson)
        {
            var duelLogs = JsonConvert
                .DeserializeObject<JObject>(duelLogJson)
                .GetValue("duel_log");

            var convertedDuelLogs = new List<DuelLog>();
            foreach (var log in duelLogs.Children<JObject>())
            {
                string endOfTheDuelDateAndTime = log.Value<string>("time");
                int roomId = log.Value<int>("roomid");

                var duelLog = new DuelLog(
                        ConvertDuelLogTimeToDateTime(endOfTheDuelDateAndTime),
                        roomId,
                        log.Value<int>("roommode"),
                        log.Value<string>("name"),
                        log.Value<string>("replay_filename")
                    );
                _addDecksFileNamesToProperColleciton(log, endOfTheDuelDateAndTime, roomId, duelLog);
                convertedDuelLogs.Add(duelLog);
            }

            return convertedDuelLogs;
        }

        /// <summary>
        /// Duel log date time format is yyyy-MM-dd hh-mm-ss.
        /// </summary>
        /// <param name="duelLogTime">Duel log time.</param>
        /// <returns>Same time, but converted to <see cref="DateTime"/>.</returns>
        public DateTime ConvertDuelLogTimeToDateTime(string duelLogTime)
        {
            string dateOfTheEndOfTheDuel = duelLogTime.Substring(0, duelLogTime.IndexOf(' '));
            var date = dateOfTheEndOfTheDuel.Split('-');

            string timeOfTheEndOfTheDuel = duelLogTime.Substring(duelLogTime.IndexOf(' '));
            var time = timeOfTheEndOfTheDuel.Split('-');

            return DateTime
                .Parse(
                    $"{date[0]}/{date[1]}/{date[2]} {time[0]}:{time[1]}:{time[2]}"
                    , CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// If deck won, we want add file name of the deck to <see cref="DuelLog.DecksWhichWonFileNames"/>
        /// <para>If deck lsot, we want add file name of the deck to <see cref="DuelLog.DecksWhichLostFileNames"/></para>
        /// </summary>
        /// <param name="log">Single duel log (not entire file, but just one log).</param>
        /// <param name="endOfTheDuelDateAndTime">Date and time end of the duel.</param>
        /// <param name="roomId">Room id from duel log.</param>
        /// <param name="duelLog">
        ///     If deck won, we want add file name of the deck to <see cref="DuelLog.DecksWhichWonFileNames"/>
        ///     <para>If deck lsot, we want add file name of the deck to <see cref="DuelLog.DecksWhichLostFileNames"/></para>
        /// </param>
        private void _addDecksFileNamesToProperColleciton(JObject log, string endOfTheDuelDateAndTime, int roomId, DuelLog duelLog)
        {
            foreach (var player in log.GetValue("players").Children())
            {
                var playerNameField = player.Value<string>("name");
                var playerName = playerNameField.Substring(0, playerNameField.IndexOf(' '));
                if (player.Value<bool>("winner"))
                {
                    duelLog.DecksWhichWonFileNames.Add($"{endOfTheDuelDateAndTime} {roomId} 1 {playerName}.ydk");
                }
                else
                {
                    duelLog.DecksWhichLostFileNames.Add($"{endOfTheDuelDateAndTime} {roomId} 0 {playerName}.ydk");
                }
            }
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Models;
using YGOProAnalyticsServer.Services.Converters.Interfaces;

namespace YGOProAnalyticsServer.Services.Converters
{
    /// <summary>
    /// It convert duel log JSON into collection of duel logs.
    /// </summary>
    public class DuelLogConverter : IDuelLogConverter
    {
        /// <inheritdoc />
        public List<DuelLog> Convert(string duelLogJson)
        {
            var duelLogs = JsonConvert
                .DeserializeObject<JObject>(duelLogJson)
                .GetValue("duel_log");
            duelLogs = duelLogs ?? throw new Exception("Is something wrong with provided JSON. Debug it if you want get more information.");

            var convertedDuelLogs = new List<DuelLog>();
            foreach (var log in duelLogs.Children<JObject>())
            {
                string beginningOfTheDuelDateAndTime = log.Value<string>("starttime");
                string endOfTheDuelDateAndTime = log.Value<string>("endtime");
                int roomId = log.Value<int>("roomid");

                var duelLog = new DuelLog(
                        ConvertDuelLogTimeToDateTime(beginningOfTheDuelDateAndTime),
                        ConvertDuelLogTimeToDateTime(endOfTheDuelDateAndTime),
                        roomId,
                        log.Value<int>("roommode"),
                        log.Value<string>("name"),
                        log.Value<string>("replay_filename")
                    );
                _addDecksFileNamesToProperColleciton(log, duelLog);
                convertedDuelLogs.Add(duelLog);
            }

            return convertedDuelLogs;
        }

        /// <inheritdoc />
        public DateTime ConvertDuelLogTimeToDateTime(string duelLogTime)
        {
            if (!Regex.IsMatch(duelLogTime, @"\d{4}-\d{2}-\d{2} \d{2}-\d{2}-\d{2}"))
            {
                throw new FormatException(_getDuelLogTimeFormatExceptionMessage(duelLogTime));
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

        /// <summary>
        /// If deck won, we want add file name of the deck to <see cref="DuelLog.DecksWhichWonFileNames"/>
        /// <para>If deck lsot, we want add file name of the deck to <see cref="DuelLog.DecksWhichLostFileNames"/></para>
        /// </summary>
        /// <param name="log">Single duel log (not entire file, but just one log).</param>
        ///     If deck won, we want add file name of the deck to <see cref="DuelLog.DecksWhichWonFileNames"/>
        ///     <para>If deck lsot, we want add file name of the deck to <see cref="DuelLog.DecksWhichLostFileNames"/></para>
        /// </param>
        private void _addDecksFileNamesToProperColleciton(JObject log, DuelLog duelLog)
        {
            foreach (var player in log.GetValue("players").Children())
            {
                if (player.Value<bool>("winner"))
                {
                    duelLog.DecksWhichWonFileNames.Add(player.Value<string>("deckname").ToString());
                }
                else
                {
                    duelLog.DecksWhichLostFileNames.Add(player.Value<string>("deckname").ToString());
                }
            }
        }

        /// <summary>
        /// Gets the duel log time format exception message.
        /// </summary>
        /// <param name="duelLogTime">The duel log time.</param>
        /// <returns>The exception message.</returns>
        private string _getDuelLogTimeFormatExceptionMessage(string duelLogTime)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("String provided as parameter is not valid duel log dateTime format.");
            stringBuilder.AppendLine("Valid format is yyyy-MM-dd HH-mm-ss.");
            stringBuilder.AppendLine($"Currently given value = {duelLogTime}.");

            return stringBuilder.ToString();
        }
    }
}

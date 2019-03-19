using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Exceptions;

namespace YGOProAnalyticsServer.Services.Analyzers
{
    /// <summary>
    /// Is responsible for getting data from duel log name.
    /// </summary>
    public class DuelLogNameAnalyzer
    {
        readonly YgoProAnalyticsDatabase _db;
        readonly IAdminConfig _config;

        public DuelLogNameAnalyzer(YgoProAnalyticsDatabase db, IAdminConfig config)
        {
            _db = db;
            _config = config;
        }

        /// <summary>
        /// It return information if duel was versus AI or not.
        /// </summary>
        /// <param name="roomName">Name of the room from duel log.</param>
        /// <returns>Information if it was duel versus AI or not.</returns>
        public bool IsDuelVersusAI(string roomName)
        {
            return Regex.IsMatch(roomName, @"^AI#\S{0,},\d{1,}")
                   || Regex.IsMatch(roomName, @"^AI\S{0,}#\d{1,}");
        }

        /// <summary>
        /// It return information if during the duel was any banlist.
        /// </summary>
        /// <param name="roomName">Name of the room from duel log.</param>
        /// <returns>Information if it is any banlist.</returns>
        public bool IsAnyBanlist(string roomName)
        {
            return
                !(
                    Regex.IsMatch(roomName, @"(\w{1,}[,^]{1}NF[,#])?(?(1)|(^NF[#,]))")
                    || IsNoDeckCheckEnabled(roomName)
                    || IsDuelVersusAI(roomName)
                );
        }

        /// <summary>
        /// It returns information if during the duel was deck check enabled.
        /// </summary>
        /// <param name="roomName">Name of the room from duel log.</param>
        /// <returns>Information if during the duel was deck check enabled</returns>
        public bool IsNoDeckCheckEnabled(string roomName)
        {
            return Regex.IsMatch(roomName, @"(\w{1,}[,^]{1}NC[,#])?(?(1)|(^NC[#,]))");
        }

        /// <summary>
        /// Get banlist based on room name and end of the duel date.
        /// </summary>
        /// <param name="roomName">Room name</param>
        /// <param name="endOfTheDuelDate">Date of the end of the duel.</param>
        /// <returns>Banlist.</returns>
        /// <exception cref="UnknownBanlistException"></exception>
        public Banlist GetBanlist(string roomName, DateTime endOfTheDuelDate)
        {
            if (IsDefaultBanlist(roomName))
            {
                var defaultBanlist = _db.Banlists
                    .Where(x => x.Name == _config.DefaultBanlsitName)
                    .FirstOrDefault();
                defaultBanlist = defaultBanlist ?? throw new UnknownBanlistException("Default banlist not found. Check in AdminConfig if it is properly set up.");

                return defaultBanlist;
            }
            else if (_isBanlistOtherThanDefaultBanlist(roomName))
            {
                int banlistNumber = (int)char.GetNumericValue(roomName[roomName.LastIndexOf("LF") + 2]);
                var banlist = _db.Banlists
                    .Where(x => x.ReleaseDate <= endOfTheDuelDate)
                    .Skip(banlistNumber - 1)
                    .FirstOrDefault();
                banlist = banlist ?? throw new UnknownBanlistException("There is no banlist in given time in our database.");

                return banlist;
            }

            throw new UnknownBanlistException(_getUnnownBanlistExceptionMessage(roomName));
        }

        /// <summary>
        /// NEVER do !_isBanlistOtherThanDefaultBanlist. Use IsDefualtBanlist instead.
        /// <see cref="IsDefaultBanlist(string)"/>
        /// </summary>
        /// <param name="roomName">Name of the room from duel log.</param>
        private bool _isBanlistOtherThanDefaultBanlist(string roomName)
        {
            return Regex.IsMatch(roomName, @"(\w{1,}[,^]{1}LF\d[,#])?(?(1)|(^LF[#,]))");
        }

        /// <summary>
        /// Check if default banlist was used during the duel.
        /// </summary>
        /// <param name="roomName">Name of the room from duel log.</param>
        /// <returns>Information if default banlist was used during the duel.</returns>
        public bool IsDefaultBanlist(string roomName)
        {
            if (IsAnyBanlist(roomName) && !_isBanlistOtherThanDefaultBanlist(roomName))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the unnown banlist exception message.
        /// </summary>
        /// <param name="roomName">Name of the room.</param>
        /// <returns>Exception message</returns>
        private string _getUnnownBanlistExceptionMessage(string roomName)
        {
            var exceptionStringBuilder = new StringBuilder();
            exceptionStringBuilder.AppendLine("Given room name do not contain information about any known banlist.");
            exceptionStringBuilder.AppendLine("Given room name is equal: ");
            exceptionStringBuilder.AppendLine(roomName);

            return exceptionStringBuilder.ToString();
        }
    }
}

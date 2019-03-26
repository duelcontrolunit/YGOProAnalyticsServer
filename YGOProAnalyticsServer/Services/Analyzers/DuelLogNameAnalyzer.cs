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
using YGOProAnalyticsServer.Services.Analyzers.Interfaces;

namespace YGOProAnalyticsServer.Services.Analyzers
{
    /// <summary>
    /// Is responsible for getting data from duel log name.
    /// </summary>
    public class DuelLogNameAnalyzer : IDuelLogNameAnalyzer
    {
        readonly YgoProAnalyticsDatabase _db;
        readonly IAdminConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="DuelLogNameAnalyzer"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="config">The configuration.</param>
        public DuelLogNameAnalyzer(YgoProAnalyticsDatabase db, IAdminConfig config)
        {
            _db = db;
            _config = config;
        }

        /// </inheritdoc>
        public bool IsDuelVersusAI(string roomName)
        {
            return Regex.IsMatch(roomName, @"^AI#\S{0,},\d{1,}")
                   || Regex.IsMatch(roomName, @"^AI\S{0,}#\d{1,}");
        }

        /// </inheritdoc>
        public bool IsAnyBanlist(string roomName)
        {
            return !IsNoDeckCheckEnabled(roomName)
                   && !IsDuelVersusAI(roomName)
                   && !_isNoBanlist(roomName);
        }

        /// </inheritdoc>
        public bool IsNoDeckCheckEnabled(string roomName)
        {
            return Regex.IsMatch(roomName, @"(\w{1,}[,^]{1}NC[,#])?(?(1)|(^NC[#,]))");
        }

        /// </inheritdoc>
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
            return Regex.IsMatch(roomName, @"(\w{1,}[,^]{1}LF\d[,#])?(?(1)|(^LF\d[#,]))");
        }

        /// </inheritdoc>
        public bool IsDefaultBanlist(string roomName)
        {
            if (IsAnyBanlist(roomName) && !_isBanlistOtherThanDefaultBanlist(roomName))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if is no banlist(no forbidden) enabled
        /// </summary>
        /// <param name="roomName">Name of the room.</param>
        /// <returns>Information if it is no forbidden (no banlist) enabled.</returns>
        private bool _isNoBanlist(string roomName)
        {
            return Regex.IsMatch(roomName, @"(\w{1,}[,^]{1}NF[,#])?(?(1)|(^NF[#,]))");
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

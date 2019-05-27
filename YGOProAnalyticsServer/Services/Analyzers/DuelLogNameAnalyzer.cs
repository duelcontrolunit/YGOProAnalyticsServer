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
using YGOProAnalyticsServer.Services.Converters.Interfaces;

namespace YGOProAnalyticsServer.Services.Analyzers
{
    /// <summary>
    /// Is responsible for getting data from duel log name.
    /// </summary>
    public class DuelLogNameAnalyzer : IDuelLogNameAnalyzer
    {
        readonly List<Banlist> _banlists;
        readonly IAdminConfig _config;
        readonly IDuelLogConverter _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DuelLogNameAnalyzer"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="config">The configuration.</param>
        public DuelLogNameAnalyzer(
            YgoProAnalyticsDatabase db, 
            IAdminConfig config,
            IDuelLogConverter converter)
        {
            _banlists = db.Banlists.ToList();
            _config = config;
            _converter = converter;
        }

        /// <inheritdoc />
        public bool IsDuelVersusAI(string roomName)
        {
            return Regex.IsMatch(roomName, @"^AI#\S{0,},\d{1,}")
                   || Regex.IsMatch(roomName, @"^AI\S{0,}#\d{1,}");
        }

        /// <inheritdoc />
        public bool IsAnyBanlist(string roomName)
        {
            return !IsNoDeckCheckEnabled(roomName)
                   && !IsDuelVersusAI(roomName)
                   && !_isNoBanlist(roomName)
                   && !IsWrongNumberBanlist(roomName);
        }

        /// <inheritdoc />
        public bool IsWrongNumberBanlist(string roomName)
        {
            if (_isBanlistOtherThanDefaultBanlist(roomName))
            {
                int banlistNumber = (int)char.GetNumericValue(roomName[roomName.LastIndexOf("LF") + 2]);
                return banlistNumber <= 0;
            }
            else
            {
                return false;
            }

        }

        /// <inheritdoc />
        public bool IsNoDeckCheckEnabled(string roomName)
        {
            return Regex.IsMatch(roomName, @"(\w{1,}[,^]{1}NC[,#])?(?(1)|(^NC[#,]))");
        }

        /// <inheritdoc />
        public Banlist GetBanlist(string roomName, DateTime endOfTheDuelFromDuelLog)
        {
            if (IsDefaultBanlist(roomName))
            {
                var defaultBanlist = _banlists
                    .Where(x => x.Name == _config.DefaultBanlistName)
                    .FirstOrDefault();
                defaultBanlist = defaultBanlist ?? throw new UnknownBanlistException("Default banlist not found. Check in AdminConfig if it is properly set up.");

                return defaultBanlist;
            }
            else if (_isBanlistOtherThanDefaultBanlist(roomName))
            {
                int banlistNumber = (int)char.GetNumericValue(roomName[roomName.LastIndexOf("LF") + 2]);
                var banlist = _banlists
                    .Where(x => x.ReleaseDate <= endOfTheDuelFromDuelLog)
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

        /// <inheritdoc />
        public bool IsDefaultBanlist(string roomName)
        {
            if (IsAnyBanlist(roomName) && !_isBanlistOtherThanDefaultBanlist(roomName))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public bool IsNoDeckShuffleEnabled(string roomName)
        {
            return Regex.IsMatch(roomName, @"(\w{1,}[,^]{1}NS[,#])?(?(1)|(^NS[#,]))");
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

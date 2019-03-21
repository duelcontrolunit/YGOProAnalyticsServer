using System;
using System.Collections.Generic;
using YGOProAnalyticsServer.Models;

namespace YGOProAnalyticsServer.Services.Converters.Interfaces
{
    /// <summary>
    /// It convert duel log JSON into collection of duel logs.
    /// </summary>
    public interface IDuelLogConverter
    {
        /// <summary>
        /// Converts the specified duel log json to list of <see cref="DuelLog"/>s.
        /// <para>Valid deck file name format is:</para>
        /// <para>yyyy-MM-dd HH-mm-ss YgoRoomIdFromDuelLog 0or1(If player won 1) nickOfTheplayer.ydk</para>
        /// </summary>
        /// <param name="duelLogJson">The duel log json.</param>
        /// <returns>List of <see cref="DuelLog"/>s</returns>
        List<DuelLog> Convert(string duelLogJson);

        /// <summary>
        /// Duel log date time format is yyyy-MM-dd hh-mm-ss.
        /// </summary>
        /// <param name="duelLogTime">Duel log time.</param>
        /// <returns>Same time, but converted to <see cref="DateTime"/>.</returns>
        DateTime ConvertDuelLogTimeToDateTime(string duelLogTime);
    }
}
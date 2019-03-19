using System;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Exceptions;

namespace YGOProAnalyticsServer.Services.Analyzers.Interfaces
{
    /// <summary>
    /// Is responsible for getting data from duel log name.
    /// </summary>
    public interface IDuelLogNameAnalyzer
    {
        /// <summary>
        /// Get banlist based on room name and end of the duel date.
        /// </summary>
        /// <param name="roomName">Room name</param>
        /// <param name="endOfTheDuelDate">Date of the end of the duel.</param>
        /// <returns>Banlist.</returns>
        /// <exception cref="UnknownBanlistException"></exception>
        Banlist GetBanlist(string roomName, DateTime endOfTheDuelDate);

        /// <summary>
        /// It return information if during the duel was any banlist.
        /// </summary>
        /// <param name="roomName">Name of the room from duel log.</param>
        /// <returns>Information if it is any banlist.</returns>
        bool IsAnyBanlist(string roomName);

        /// <summary>
        /// Check if default banlist was used during the duel.
        /// </summary>
        /// <param name="roomName">Name of the room from duel log.</param>
        /// <returns>Information if default banlist was used during the duel.</returns>
        bool IsDefaultBanlist(string roomName);

        /// <summary>
        /// It return information if duel was versus AI or not.
        /// </summary>
        /// <param name="roomName">Name of the room from duel log.</param>
        /// <returns>Information if it was duel versus AI or not.</returns>
        bool IsDuelVersusAI(string roomName);

        /// <summary>
        /// It returns information if during the duel was deck check enabled.
        /// </summary>
        /// <param name="roomName">Name of the room from duel log.</param>
        /// <returns>Information if during the duel was deck check enabled</returns>
        bool IsNoDeckCheckEnabled(string roomName);
    }
}
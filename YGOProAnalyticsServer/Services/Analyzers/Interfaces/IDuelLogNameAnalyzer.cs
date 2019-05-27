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
        /// <param name="beginningOfTheDuelDate">Date of the start of the duel.</param>
        /// <returns>Banlist.</returns>
        /// <exception cref="UnknownBanlistException"></exception>
        Banlist GetBanlist(string roomName, DateTime beginningOfTheDuelDate);

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

        /// <summary>
        /// Information if is no deck shuffle enabled.
        /// </summary>
        /// <param name="roomName">Name of the room.</param>
        /// <returns>
        ///  Information if is no deck shuffle enabled.
        /// </returns>
        bool IsNoDeckShuffleEnabled(string roomName);
        /// <summary>
        /// Determines whether a negative or 0 number was used in roomName.
        /// </summary>
        /// <param name="roomName">Name of the room.</param>
        /// <returns>
        ///   <c>true</c> if a negative or 0 number wass used in roomName otherwise, <c>false</c>.
        /// </returns>
        bool IsWrongNumberBanlist(string roomName);
    }
}
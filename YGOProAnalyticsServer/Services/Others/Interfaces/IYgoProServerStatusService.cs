using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Services.Others.Interfaces
{
    /// <summary>
    /// Provide informations about server status.
    /// </summary>
    public interface IYgoProServerStatusService
    {
        /// <summary>
        /// Check if server is online.
        /// </summary>
        /// <param name="url">List of the rooms endpoint.</param>
        /// <returns>Information if server is online or not.</returns>
        Task<bool> IsOnlineBasedOnListOfRooms(string url);

        /// <summary>
        /// Numbers of the closed rooms.
        /// </summary>
        /// <param name="url">List of the rooms endpoint.</param>
        /// <returns>Number of closed rooms.</returns>
        Task<int> NumberOfClosedRooms(string url);

        /// <summary>
        /// Numbers of the open rooms.
        /// </summary>
        /// <param name="url">List of the rooms endpoint.</param>
        /// <returns>Number of open rooms.</returns>
        Task<int> NumberOfOpenRooms(string url);

        /// <summary>
        /// Numbers of the players in entire game.
        /// </summary>
        /// <param name="url">List of the rooms endpoint.</param>
        /// <returns>Number of players.</returns>
        Task<int> NumberOfPlayersInAllRooms(string url);

        /// <summary>
        /// Numbers of the players in lobby.
        /// </summary>
        /// <param name="url">List of the rooms endpoint.</param>
        /// <returns>Numbers of the players in lobby.</returns>
        Task<int> NumberOfPlayersInLobby(string url);

        /// <summary>
        /// Numbers of the players which play now.
        /// </summary>
        /// <param name="url">List of the rooms endpoint.</param>
        /// <returns>Numbers of the players which play now.</returns>
        Task<int> NumberOfPlayersWhichPlayNow(string url);

        /// <summary>
        /// Numbers of rooms in entire game.
        /// </summary>
        /// <param name="url">List of the rooms endpoint.</param>
        /// <returns>Numbers of rooms in entire game.</returns>
        Task<int> NumberOfAllRooms(string url);
    }
}
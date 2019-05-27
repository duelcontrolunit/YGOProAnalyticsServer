using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Models
{
    /// <summary>
    /// Representation of duel log given from server.
    /// </summary>
    public class DuelLog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuelLog"/> class.
        /// </summary>
        /// <param name="dateOfTheEndOfTheDuel">The date of the end of the duel.</param>
        /// <param name="roomId">The room identifier.</param>
        /// <param name="roomMode">The room mode. For example: Single (0), Match(1), Tag duel(2)</param>
        /// <param name="name">The name.</param>
        /// <param name="replayFilename">The replay filename.</param>
        /// <exception cref="ArgumentNullException">
        /// name
        /// or
        /// replayFilename
        /// </exception>
        public DuelLog( 
            DateTime dateOfTheBeginningOfTheDuel,
            DateTime dateOfTheEndOfTheDuel, 
            int roomId, 
            int roomMode, 
            string name, 
            string replayFilename)
        {
            DateOfTheBeginningOfTheDuel = dateOfTheBeginningOfTheDuel;
            DateOfTheEndOfTheDuel = dateOfTheEndOfTheDuel;
            RoomId = roomId;
            RoomMode = roomMode;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ReplayFilename = replayFilename ?? throw new ArgumentNullException(nameof(replayFilename));
            DecksWhichWonFileNames = new List<string>();
            DecksWhichLostFileNames = new List<string>();
        }
        /// <summary>
        /// The date of the beginning of the duel.
        /// </summary>
        public DateTime DateOfTheBeginningOfTheDuel { get; protected set; }

        /// <summary>
        /// The date of the end of the duel.
        /// </summary>
        public DateTime DateOfTheEndOfTheDuel { get; protected set; }

        /// <summary>
        /// The room identifier.
        /// </summary>
        public int RoomId { get; protected set; }

        /// <summary>
        /// Room mode.
        /// </summary>
        public int RoomMode { get; protected set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The replay filename.
        /// </summary>
        public string ReplayFilename { get; protected set; }

        /// <summary>
        /// The decks which won file names.
        /// </summary>
        public List<string> DecksWhichWonFileNames { get; protected set; }

        /// <summary>
        /// The decks which lost file names.
        /// </summary>
        public List<string> DecksWhichLostFileNames { get; protected set; }
    }
}

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
        /// The identifier.
        /// </summary>
        public int Id { get; protected set; }

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
        public int RoomMode { get; set; }

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

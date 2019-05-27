using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class CurrentServerActivityDTO
    {
        public int NumberOfPlayersInLobbyNow { get; set; }
        public int NumberOfPlayersInDuelNow { get; set; }
        public int NumberOfRoomsInLobbyNow { get; set; }
        public int NumberOfRoomsInDuelNow { get; set; }
    }
}

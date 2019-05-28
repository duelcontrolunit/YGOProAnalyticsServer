using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class YgoProActivityNowDTO
    {
        public int NumberOfOpenRooms { get; set; }
        public int NumberOfClosedRooms { get; set; }
        public int NumberOfPlayersInLobby { get; set; }
        public int NumberOfPlayersWhichPlayNow { get; set; }
    }
}

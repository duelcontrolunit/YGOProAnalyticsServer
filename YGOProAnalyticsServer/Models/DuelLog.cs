using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Models
{
    public class DuelLog
    {
        public int Id { get; protected set; }
        public DateTime DateOfTheEndOfTheDuel { get; protected set; }
        public int RoomId { get; protected set; }
        public int RoomMode { get; set; }
        public string Name { get; protected set; }
        public string ReplayFilename { get; protected set; }
        public List<string> DecksWhichWonFileNames { get; protected set; }
        public List<string> DecksWhichLostFileNames { get; protected set; }
    }
}

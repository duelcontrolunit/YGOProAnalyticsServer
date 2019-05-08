using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Models;

namespace YGOProAnalyticsServer.Events
{
    public class DataFromYgoProServerRetrieved : INotification
    {
        private Dictionary<DateTime, List<DuelLog>> ConvertedDuelLogs { get; }
        private Dictionary<DateTime, List<DecklistWithName>> UnzippedDecklistsWithDecklistFileName { get; }

        public DataFromYgoProServerRetrieved(Dictionary<DateTime, List<DuelLog>> convertedDuelLogs, Dictionary<DateTime, List<DecklistWithName>> unzippedDecklistsWithDecklistFileName)
        {
            ConvertedDuelLogs = convertedDuelLogs;
            UnzippedDecklistsWithDecklistFileName = unzippedDecklistsWithDecklistFileName;
        }
    }
}

﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Models;

namespace YGOProAnalyticsServer.Events
{
    public class DataFromYgoProServerRetrieved : INotification
    {
        public Dictionary<DateTime, List<DuelLog>> ConvertedDuelLogs { get; }
        public Dictionary<DateTime, List<DecklistWithName>> UnzippedDecklistsWithDecklistFileName { get; }

        public IEnumerable<Banlist> NewBanlists { get; }

        public DataFromYgoProServerRetrieved(
            Dictionary<DateTime, List<DuelLog>> convertedDuelLogs,
            Dictionary<DateTime, List<DecklistWithName>> unzippedDecklistsWithDecklistFileName,
            IEnumerable<Banlist> updatedBanlists)
        {
            ConvertedDuelLogs = convertedDuelLogs;
            UnzippedDecklistsWithDecklistFileName = unzippedDecklistsWithDecklistFileName;
            NewBanlists = updatedBanlists;
        }
    }
}

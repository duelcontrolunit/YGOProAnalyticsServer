using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.DTOs
{
    public class DecklistWithStatisticsDTO
    {
        public int DecklistId { get; set; }
        public string Name { get; set; }
        public string Archetype { get; set; }
        public DateTime WhenDeckWasFirstPlayed { get; set; }

        public MainDeckDTO MainDeck { get; set; }
        public ExtraDeckDTO ExtraDeck { get; set; }
        public DeckDTO SideDeck { get; set; }
        public IEnumerable<DeckStatisticsDTO> Statistics { get; set; }
    }
}

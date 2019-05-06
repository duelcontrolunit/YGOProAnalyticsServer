using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.DTOs
{
    public class BanlistWithStatisticsDTO
    {
        public string Name { get; set; }
        public string Format { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DeckDTO BannedCards { get; set; }
        public DeckDTO LimitedCards { get; set; }
        public DeckDTO SemiLimitedCards { get; set; }
        public List<BanlistStatisticsDTO> Statistics { get; set; }
    }
}

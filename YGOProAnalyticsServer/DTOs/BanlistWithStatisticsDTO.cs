using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.DTOs
{
    public class BanlistWithStatisticsDTO
    {
        public BanlistWithStatisticsDTO(
            string name,
            string format,
            DateTime releaseDate,
            DeckDTO bannedCards,
            DeckDTO limitedCards,
            DeckDTO semiLimitedCards,
            List<BanlistStatisticsDTO> statistics)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Format = format ?? throw new ArgumentNullException(nameof(format));
            ReleaseDate = releaseDate;
            BannedCards = bannedCards ?? throw new ArgumentNullException(nameof(bannedCards));
            LimitedCards = limitedCards ?? throw new ArgumentNullException(nameof(limitedCards));
            SemiLimitedCards = semiLimitedCards ?? throw new ArgumentNullException(nameof(semiLimitedCards));
            Statistics = statistics ?? throw new ArgumentNullException(nameof(statistics));
        }

        public string Name { get; set; }
        public string Format { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DeckDTO BannedCards { get; set; }
        public DeckDTO LimitedCards { get; set; }
        public DeckDTO SemiLimitedCards { get; set; }
        public List<BanlistStatisticsDTO> Statistics { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class BanlistBrowserResultsDTO
    {
        public BanlistBrowserResultsDTO(int totalNumberOfPages,
            IEnumerable<BanlistWithHowManyWasUsed> banlists)
        {
            TotalNumberOfPages = totalNumberOfPages;
            Banlists = banlists ?? throw new ArgumentNullException(nameof(banlists));
        }

        public int TotalNumberOfPages { get; set; }

        public IEnumerable<BanlistWithHowManyWasUsed> Banlists { get; set; }
    }
}
